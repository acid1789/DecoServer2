using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using DecoServer2.CharacterThings;
using DecoServer2.Quests;

namespace JuggleServerCore
{
    public class Connection
    {
        public enum ConnStatus
        {
            New,
            Connecting,
            Connected,
            Disconnected,
            Closed
        }

        #region Packet Events
        public event EventHandler<LoginRequestPacket> OnLoginRequest;
        public event EventHandler<CharacterListRequestPacket> OnCharacterListRequest;
        public event EventHandler<CreateCharacterPacket> OnCreateCharacter;
        public event EventHandler<CharacterNameClass> OnDeleteCharacter;
        public event EventHandler<CharacterNameClass> OnSelectCharacter;
        public event EventHandler OnLoadSelectedCharacter;
        public event EventHandler OnPlayerEnterMap;
        public event EventHandler<CharacterPositionClass> OnMoveTo;
        public event EventHandler<CharacterPositionClass> OnUpdatePosition;
        public event EventHandler OnNPCDialogNextButton;
        #endregion

        #region Quest Events
        public event EventHandler<QuestDialogFinishedArgs> OnQuestDialogFinished;
        #endregion

        Socket _socket;
        ConnStatus _conStatus;
        uint _sequence;
        int _accountId;
        
        PacketSecurity _scSec = new PacketSecurity(true);
        PacketSecurity _csSec = new PacketSecurity(false);

        delegate void PacketHandler(PacketHeader hdr, BinaryReader br);
        Dictionary<ushort, PacketHandler> _packetHandlers;
        
        public Connection()
        {
            Init();
            _conStatus = ConnStatus.New;
        }

        public Connection(Socket connectedSocket)
        {
            Init();
            _socket = connectedSocket;
            _conStatus = ConnStatus.Connected;
        }

        void Init()
        {
            _sequence = 0;

            _packetHandlers = new Dictionary<ushort, PacketHandler>();
            _packetHandlers[0x0002] = CreateCharacter_Handler;
            _packetHandlers[0x0004] = DeleteCharacter_Handler;
            _packetHandlers[0x0010] = SelectCharacter_Handler;
            _packetHandlers[0x0014] = LoadSelectedCharacter_Handler;
            _packetHandlers[0x0121] = MoveTo_Handler;
            _packetHandlers[0x0124] = UpdatePosition_Handler;
            _packetHandlers[0x0133] = PlayerEnterMap_Handler;
            _packetHandlers[0x0257] = NextNPCDialogButton_Handler;
            _packetHandlers[0x7FD3] = LoginRequest_Handler;
            _packetHandlers[0x7FD4] = CharacterListRequest_Handler;
        }

        public void Disconnect()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Close();                
            }
            _conStatus = ConnStatus.Disconnected;
        }

        public void Update()
        {
            if (_conStatus == ConnStatus.Connected)
            {
                if (_socket.Available > 0)
                {
                    byte[] data = new byte[_socket.Available];
                    int bytesRead = _socket.Receive(data);
                    ProcessPackets(data, bytesRead);
                }
            }
        }

        void ProcessPackets(byte[] data, int totalBytes)
        {
            int offset = 0;
            while (offset < totalBytes)
            {
                offset += ProcessPacket(data, offset);
            }
        }

        int ProcessPacket(byte[] data, int offset)
        {
            ushort verify = BitConverter.ToUInt16(data, 0);
            int dataLength = data[offset] | data[offset + 1] << 8;
            byte[] decrypted = _csSec.DecryptPacket(data, offset + 2, dataLength);

            // Grab the packet header
            MemoryStream mem = new MemoryStream(decrypted);
            BinaryReader br = new BinaryReader(mem);
            PacketHeader header = PacketHeader.Read(br);

            LogInterface.Log(string.Format("Recieved packet from client ({0}): 0x{1:x}", AccountID, header.Opcode), LogInterface.LogMessageType.Debug);

            // Dispatch the packet for further processing
            if (_packetHandlers.ContainsKey(header.Opcode))
                _packetHandlers[header.Opcode](header, br);
            else
            {
                LogInterface.Log(string.Format("Unhandled packet 0x{0:x} - Length: {1}", header.Opcode, header.PacketLength), LogInterface.LogMessageType.Error, true);
                DumpPacketData(header, br);
            }

            return dataLength + 2;
        }

        public void SendPacket(SendPacketBase packet)
        {
            if (!_socket.Connected)
            {
                _conStatus = ConnStatus.Disconnected;
                return;
            }
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            packet.Write(_sequence++, bw);
            byte[] data = ms.ToArray();

            LogInterface.Log(string.Format("Sent Packet to client ({0}) {1}: {2}", AccountID, packet.ToString(), data.Length), LogInterface.LogMessageType.Debug);
            LogInterface.Log(Utils.PrintBinaryData(data), LogInterface.LogMessageType.Debug);

            _scSec.EncryptInPlace(data);

            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
            bw.Write((ushort)data.Length);
            bw.Write(data);
            _socket.Send(ms.ToArray());
            bw.Close();

        }

        public void SendEmptyPacket(ushort opcode)
        {
            SendPacket(new EmptyPacket(opcode) );
        }

        #region Character Data
        CharacterSelectInfo[] _charSelectSlots;
        int _selectedCharacterID;
        CharacterInfo _ci;

        public void SetCharacterIDs(CharacterSelectInfo[] chars)
        {
            _charSelectSlots = chars;
        }

        public int GetCharacterID(string charName)
        {
            if (_charSelectSlots != null)
            {
                for (int i = 0; i < _charSelectSlots.Length; i++)
                {
                    if( _charSelectSlots[i].IsValid && _charSelectSlots[i].Name == charName )
                        return i;
                }
            }

            return -1;
        }

        public int SelectCharacter(string charName)
        {
            _selectedCharacterID = GetCharacterID(charName);
            return _selectedCharacterID;
        }

        public CharacterInfo LoadSelectedCharacter()
        {
            _ci = new CharacterInfo(_charSelectSlots[_selectedCharacterID]);
            _charSelectSlots = null;    // Throw away the select data for characters we arent using
            return _ci;
        }
        #endregion

        #region Game State Stuff
        uint _currentQuestNPC;
        uint _currentQuestID;
        byte _currentQuestLine;
        public void SetCurrentQuest(uint npcID, uint questID, byte line)
        {
            _currentQuestNPC = npcID;
            _currentQuestID = questID;
            _currentQuestLine = line;
        }

        public void QuestDialogFinished(uint npcID)
        {
            if( OnQuestDialogFinished != null )
                OnQuestDialogFinished(this, new QuestDialogFinishedArgs(npcID));
        }
        #endregion

        #region PacketHandlers
        void CreateCharacter_Handler(PacketHeader header, BinaryReader br)
        {
            OnCreateCharacter(this, CreateCharacterPacket.Read(header, br));
        }

        void DeleteCharacter_Handler(PacketHeader header, BinaryReader br)
        {
            OnDeleteCharacter(this, CharacterNameClass.Read(header, br));
        }

        void SelectCharacter_Handler(PacketHeader header, BinaryReader br)
        {
            OnSelectCharacter(this, CharacterNameClass.Read(header, br));
        }

        void LoadSelectedCharacter_Handler(PacketHeader header, BinaryReader br)
        {
            OnLoadSelectedCharacter(this, null);
        }

        void PlayerEnterMap_Handler(PacketHeader header, BinaryReader br)
        {
            OnPlayerEnterMap(this, null);
        }

        void MoveTo_Handler(PacketHeader header, BinaryReader br)
        {
            OnMoveTo(this, CharacterPositionClass.Read(header, br));
        }

        void UpdatePosition_Handler(PacketHeader header, BinaryReader br)
        {
            OnUpdatePosition(this, CharacterPositionClass.Read(header, br));
        }

        void LoginRequest_Handler(PacketHeader header, BinaryReader br)
        {
            LoginRequestPacket lrp = LoginRequestPacket.Read(header, br);
            OnLoginRequest(this, lrp);
        }

        void CharacterListRequest_Handler(PacketHeader header, BinaryReader br)
        {
            OnCharacterListRequest(this, CharacterListRequestPacket.Read(header, br));
        }

        void NextNPCDialogButton_Handler(PacketHeader header, BinaryReader br)
        {
            OnNPCDialogNextButton(this, null);
        }
        #endregion

        void DumpPacketData(PacketHeader header, BinaryReader br)
        {
            byte[] data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));            
            string output = Utils.PrintBinaryData(data);
            LogInterface.Log(output, LogInterface.LogMessageType.Debug, true);
        }

        #region Accessors
        public ConnStatus Status
        {
            get { return _conStatus; }
        }

        public int AccountID
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        public int SelectedCharacter
        {
            get { return _selectedCharacterID; }
        }

        public CharacterInfo Character
        {
            get { return _ci; }
        }

        public uint CurrentQuestNPC
        {
            get { return _currentQuestNPC; }
        }
        
        public uint CurrentQuestID
        {
            get { return _currentQuestID; }
        }

        public byte CurrentQuestLine
        {
            get { return _currentQuestLine; }
        }
        #endregion
    }
}
