using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DecoServer2.CharacterThings;

namespace JuggleServerCore
{
    public class PacketHeader
    {
        public byte SecurityBytes;          // 0x0
        public ushort PacketLength;         // 0x1
        public ushort Opcode;               // 0x3
        public uint PacketSequenceNumber;   // 0x5    // what a waste of space
        public byte HeaderLength;           // 0x9:   // always 14 duh.
        public uint Unknown;                // 0xA:

        public PacketHeader()
        {
            SecurityBytes = 0;
            HeaderLength = 14;
            Unknown = 1;
        }

        public static PacketHeader Read(BinaryReader br)
        {
            PacketHeader header = new PacketHeader();
            header.SecurityBytes = br.ReadByte();
            header.PacketLength = br.ReadUInt16();
            header.Opcode = (ushort)(br.ReadUInt16() >> 1);
            header.PacketSequenceNumber = br.ReadUInt32();
            header.HeaderLength = br.ReadByte();
            header.Unknown = br.ReadUInt32();

            return header;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(SecurityBytes);
            bw.Write(PacketLength);
            bw.Write((ushort)(Opcode << 1));
            bw.Write(PacketSequenceNumber);
            bw.Write(HeaderLength);
            bw.Write(Unknown);
        }
    }

    #region Packets From Client
    public class CreateCharacterPacket : EventArgs      // 0x2
    {
        public string Name;
        public int ModelInfo;

        public static CreateCharacterPacket Read(PacketHeader header, BinaryReader br)
        {
            CreateCharacterPacket ccp = new CreateCharacterPacket();
            br.ReadInt32();     // Unknown 0x0A000000
            ccp.Name = Utils.ReadByteString(br, 17);
            ccp.ModelInfo = br.ReadInt32();
            return ccp;
        }
    }

    public class CharacterNameClass : EventArgs      // 0x4
    {
        public string Name;
        public static CharacterNameClass Read(PacketHeader header, BinaryReader br)
        {
            CharacterNameClass dcp = new CharacterNameClass();
            dcp.Name = Utils.ReadByteString(br, 17);
            return dcp;
        }
    }

    public class CharacterPositionClass : EventArgs           // 0x121
    {
        public uint CellIndex;
        public static CharacterPositionClass Read(PacketHeader header, BinaryReader br)
        {
            CharacterPositionClass mtp = new CharacterPositionClass();
            mtp.CellIndex = br.ReadUInt32();
            return mtp;
        }
    }

    public class MoveItemRequest : EventArgs                   // 0x453
    {
        public uint ItemID;
        public uint OtherID;
        public byte Slot;

        public static MoveItemRequest Read(PacketHeader header, BinaryReader br)
        {
            MoveItemRequest mir = new MoveItemRequest();
            mir.ItemID = br.ReadUInt32();
            mir.OtherID = br.ReadUInt32();
            mir.Slot = br.ReadByte();
            return mir;
        }
    }

    public class GMCommandPacket : EventArgs
    {
        public uint Command;
        public int Param;
        public int X;
        public int Y;
        public int Param2;
        public string Character;

        public static GMCommandPacket Read(PacketHeader header, BinaryReader br)
        {
            GMCommandPacket pkt = new GMCommandPacket();
            pkt.Command = br.ReadUInt32();
            pkt.Param = br.ReadInt32();
            pkt.X = br.ReadInt32();
            pkt.Y = br.ReadInt32();
            pkt.Param2 = br.ReadInt32();
            pkt.Character = Utils.ReadByteString(br, 0x1F);
            return pkt;
        }
    }

    public class LoginRequestPacket : EventArgs //  0x7FD3
    {
        public PacketHeader Header;
        public string UserName;
        public string Password;

        public static LoginRequestPacket Read(PacketHeader header, BinaryReader br)
        {
            LoginRequestPacket lrp = new LoginRequestPacket();
            lrp.Header = header;
            lrp.UserName = Utils.ReadByteString(br, 65);
            lrp.Password = Utils.ReadByteString(br, 65);

            return lrp;
        }
    }

    public class CharacterListRequestPacket : EventArgs // 0x7FD4
    {
        public PacketHeader Header;
        public string AuthKey;

        public static CharacterListRequestPacket Read(PacketHeader header, BinaryReader br)
        {
            CharacterListRequestPacket clrp = new CharacterListRequestPacket();
            clrp.AuthKey = Utils.ReadByteString(br, (int)(br.BaseStream.Length - br.BaseStream.Position));
            return clrp;
        }
    }
    #endregion

    #region Packets To Client
    public abstract class SendPacketBase
    {
        public abstract void Write(uint sequence, BinaryWriter bw);
    }

    public class EmptyPacket : SendPacketBase
    {
        ushort Opcode;
        public EmptyPacket(ushort opcode)
        {
            Opcode = opcode;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = Opcode;
            header.PacketLength = (ushort)(0);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);
        }

        public override string ToString()
        {
            return string.Format("EmptyPacket({0})", Opcode);
        }
    }

    public class ErrorMessagePacket : SendPacketBase            // 0x0
    {
        public string Message;

        public ErrorMessagePacket(string message)
        {
            Message = message;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(Message);

            PacketHeader header = new PacketHeader();
            header.Opcode = 0;
            header.PacketLength = (ushort)(ascii.Length + 5);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);
                        
            bw.Write((int)0);   // Message Type: Server Provided
            bw.Write(ascii);
            bw.Write((byte)0);
        }
    }

    public class CharacterListPacket : SendPacketBase           // 0x1
    {
        public CharacterSelectInfo[] Slots;

        public CharacterListPacket(CharacterSelectInfo[] slots)
        {
            Slots = slots;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 1;
            header.PacketLength = (ushort)(400);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            bw.Write((uint)2);  // 2 Character slots... Retarded client.  Can do a third but character creation fails...
            for( int i = 0; i < 3; i++ )
                Slots[i].Write(bw);
        }
    }

    public class DeleteCharacterConfirmPacket : SendPacketBase  // 0x5
    {
        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x5;
            header.PacketLength = (ushort)(32);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            for( int i = 0; i < 8; i++ )
                bw.Write((int)1);
        }
    }

    public class SelectCharacterIDPacket : SendPacketBase // 0x13
    {
        int _id;
        public SelectCharacterIDPacket(int id)
        {
            _id = id;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x13;
            header.PacketLength = 4;
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            bw.Write((int)1);
            //bw.Write(_id);
        }
    }

    public class MapChangePacket : SendPacketBase       // 0x15
    {
        CharacterInfo _ci;

        public MapChangePacket(CharacterInfo ci)
        {
            _ci = ci;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x15;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = _ci.DataSize;
            header.Write(bw);

            _ci.WritePacket(bw);
        }
    }

    public class PlayerMovePacket : SendPacketBase  // 0x0122
    {
        uint _cellIndex;
        ushort _moveSpeed;
        public PlayerMovePacket(uint cellIndex, ushort moveSpeed)
        {
            _cellIndex = cellIndex;
            _moveSpeed = moveSpeed;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x122;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 7;
            header.Write(bw);

            bw.Write(_cellIndex);
            bw.Write(_moveSpeed);
            bw.Write((byte)1);
        }
    }

    public class ObserveMovementPacket : SendPacketBase // 0x0123
    {
        uint _moverID;
        uint _cellIndex;
        ushort _speed;
        public ObserveMovementPacket(uint worldID, uint cellIndex, ushort speed)
        {
            _moverID = worldID;
            _cellIndex = cellIndex;
            _speed = speed;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0123;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 10;
            header.Write(bw);

            bw.Write(_moverID);
            bw.Write(_cellIndex);
            bw.Write(_speed);
        }
    }

    public class NPCDialogPacket : SendPacketBase                   // 0x0252     
    {
        ushort _npcType;
        public NPCDialogPacket(ushort npcType)
        {
            _npcType = npcType;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0252;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 3;
            header.Write(bw);

            bw.Write((byte)1);
            bw.Write(_npcType);
        }
    }

    public class WindowSettingsPacket_NPCIcon : SendPacketBase      // 0x0255
    {
        ushort _icon;
        public WindowSettingsPacket_NPCIcon(ushort icon)
        {
            _icon = icon;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0255;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 3;
            header.Write(bw);
            
            bw.Write((byte)11);     // NPC_Icon
            bw.Write(_icon);        // Icon texture id
        }
    }

    public class WindowSettingsPacket_NPCDialog : SendPacketBase    // 0x0255
    {
        uint _staticText;
        string _text;
        bool _showNext;
        public WindowSettingsPacket_NPCDialog(uint staticText, string text, bool showNextButton = true)
        {
            _staticText = staticText;
            _text = text;
            _showNext = showNextButton;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0255;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 31;
            header.Write(bw);

            bw.Write((byte)21);                     // NPC Quest Dialog Text
            bw.Write((ushort)(_showNext ? 1 : 0));    // Should the dialog have the next button?
            bw.Write(_staticText);
            Utils.WriteByteString(bw, _text, 24);
        }
    }

    public class MoveItemResponse : SendPacketBase  // 0x0454
    {
        uint _id;
        uint _other;
        byte _slot;
        bool _success;

        public MoveItemResponse(uint id, uint other, byte slot, bool success)
        {
            _id = id;
            _other = other;
            _slot = slot;
            _success = success;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0454;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 11;
            header.Write(bw);

            bw.Write(_id);
            bw.Write(_other);
            bw.Write(_slot);
            bw.Write((byte)1);  // Unknown
            bw.Write((byte)(_success ? 1 : 0));
        }
    }

    public class GiveItemPacket : SendPacketBase    // 0x0455
    {
        Item _item;

        public GiveItemPacket(Item item)
        {
            _item = item;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0445;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 17;
            header.Write(bw);

            bw.Write((byte)1);          // Error code: 1 = no error
            _item.Write(bw);
        }
    }

    public class NPCInfoPacket : SendPacketBase     // 0x5002
    {
        DecoServer2.NPC _npc;

        public NPCInfoPacket(DecoServer2.NPC npc)
        {
            _npc = npc;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x5002;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = _npc.DataSize;
            header.Write(bw);
            
            _npc.Write(bw);
        }
    }

    public class LoginResponsePacket : SendPacketBase           // 0x7FD2
    {
        string Unknown;

        public LoginResponsePacket(string unk)
        {
            Unknown = unk;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0;
            header.PacketLength = (ushort)(62);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            Utils.WriteByteString(bw, Unknown, 31);
            Utils.WriteByteString(bw, "test", 31);
        }
    }

    public class LoginErrorPacket : SendPacketBase              // 0x7FD9
    {
        public enum ErrorCodes
        {
            AccountDoesntExist,
            PasswordDoesntMatch,
        }

        public ErrorCodes ErrorCode;
        public string AccountURL;

        public LoginErrorPacket(ErrorCodes code, string accountURL)
        {
            ErrorCode = code;
            AccountURL = accountURL;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x7FD9;
            header.PacketLength = (ushort)(404);
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            int error = (ErrorCode == ErrorCodes.AccountDoesntExist) ? 2 : 3;
            bw.Write(error);
            Utils.WriteZeros(bw, 200);
            Utils.WriteByteString(bw, AccountURL, 200);
        }
    }

    public class ServerListPacket  : SendPacketBase           // 0x7FDA
    {
        PlayServerInfo[] _servers;
        string _authKey;

        public ServerListPacket(PlayServerInfo[] servers, string authKey)
        {
            _servers = servers;
            _authKey = authKey;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            const int serverSize = 74;
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x7FDA;
            header.PacketLength =(ushort)(35 + (_servers.Length * serverSize));
            header.PacketSequenceNumber = sequence;

            header.Write(bw);
            Utils.WriteByteString(bw, _authKey, 31);
            bw.Write((uint)_servers.Length);
            foreach (PlayServerInfo psi in _servers)
            {
                Utils.WriteZeros(bw, 11);       // Unknown data, this might not belong in the loop
                
                bw.Write((ushort)1);            // Unknown
                Utils.WriteByteString(bw, psi.Name, 40);
                Utils.WriteByteString(bw, psi.IPAddress, 16);
                bw.Write(psi.Port);
                bw.Write((ushort)0);            // Unknown
                bw.Write((byte)1);              // Server Population
            }
        }
    }
    #endregion
}
