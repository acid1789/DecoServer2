﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DecoServer2.CharacterThings;
using DecoServer2;

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

    public class AttackTargetRequest : EventArgs                // 0x301
    {
        public enum TargetType
        {
            Monster,
            Player
        };

        enum ComboType
        {
        };

        public uint TargetID;
        public TargetType TargetT;
        public int ComboT;
        public int ComboCount;
        public int ComboStatus;
        public ushort Motion;
        public bool Critical;

        public static AttackTargetRequest Read(PacketHeader header, BinaryReader br)
        {
            AttackTargetRequest atr = new AttackTargetRequest();

            atr.TargetID = br.ReadUInt32();
            atr.Critical = false;

            byte typeByte = br.ReadByte();
            atr.TargetT = (typeByte & 3) == 0 ? TargetType.Player : TargetType.Monster;
            atr.ComboT = ((typeByte >> 2) & 0x3F);

            byte unkByte = br.ReadByte();

            byte comboByte = br.ReadByte();
            atr.ComboCount = (comboByte & 0x3F);
            atr.ComboStatus = ((comboByte >> 6) & 0x3);

            atr.Motion = br.ReadUInt16();

            return atr;
        }
    }

    public class EquipItemRequest : EventArgs                   // 0x411
    {
        public uint ItemID;
        public byte Slot;

        public static EquipItemRequest Read(PacketHeader header, BinaryReader br)
        {
            EquipItemRequest eir = new EquipItemRequest();
            eir.ItemID = br.ReadUInt32();
            eir.Slot = br.ReadByte();
            return eir;
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

    public class ToolbarItemSetRequest : EventArgs                     // 0x455
    {
        public enum TargetType
        {
            Item,
            Skill,
            Unknown
        }

        public uint TargetID;
        public TargetType Type;
        public int Slot;

        public static ToolbarItemSetRequest Read(PacketHeader header, BinaryReader br)
        {
            uint id = br.ReadUInt32();
            byte extra = br.ReadByte();
            TargetType t = TargetType.Unknown;
            switch (extra & 0x3)
            {
                case 2:
                    t = TargetType.Item;
                    break;
                case 3:
                    t = TargetType.Skill;
                    id &= 0xFFFF;
                    break;
            }

            ToolbarItemSetRequest tbr = new ToolbarItemSetRequest();
            tbr.TargetID = id;
            tbr.Type = t;
            tbr.Slot = extra >> 2;

            return tbr;
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
            for (int i = 0; i < 3; i++)
                Slots[i].Write(bw);
        }
    }

    public class DeleteCharacterConfirmPacket : SendPacketBase  // 0x5
    {
        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x5;
            header.PacketLength = 32;
            header.PacketSequenceNumber = sequence;
            header.Write(bw);

            for (int i = 0; i < 8; i++)
                bw.Write((int)1);
        }
    }

    public class AddOtherPlayerPacket : SendPacketBase          // 0xF
    {
        CharacterInfo _op;

        public AddOtherPlayerPacket(CharacterInfo other)
        {
            _op = other;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0xF;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 258;
            header.Write(bw);

            bw.Write(_op.ID);
            Utils.WriteByteString(bw, _op.Name, 17);
            bw.Write(_op.ModelInfo);
            bw.Write(_op.Job);

            bw.Write((byte)0);
            bw.Write((uint)0);

            bw.Write(_op.Level);
            bw.Write(_op.CellIndex);
            bw.Write(_op.CurHP);
            bw.Write(_op.CurMP);
            bw.Write(_op.MaxHP);
            bw.Write(_op.MaxMP);

            bw.Write((byte)0);
            bw.Write((ushort)1);

            for (int i = 0; i < 10; i++)
            {
                int id = 0;
                Item equipped = _op.EquippedItem((byte)i);
                if( equipped != null )
                    id = equipped.Model;
                bw.Write(id);
            }

            bw.Write((uint)0);
            bw.Write((uint)0);
            bw.Write((uint)0);
            bw.Write((uint)0);
            for( int i = 0; i < 20; i++ )
                bw.Write((ushort)0);
            bw.Write((ushort)0);
            bw.Write((uint)0);
            bw.Write((byte)0);
            Utils.WriteByteString(bw, "", 100);
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

    public class HealthChangePacket : SendPacketBase    // 0xFF
    {
        uint _hp;

        public HealthChangePacket(uint health)
        {
            _hp = health;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0xFF;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 4;
            header.Write(bw);

            bw.Write(_hp);
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

    public class SeePlayerAttack : SendPacketBase       // 0x0302
    {
        uint _victimID;
        uint _victimHP;
        CharacterInfo _ci;
        AttackTargetRequest _atr;
        byte _errorByte;

        public SeePlayerAttack(uint victimID, uint victimHP, CharacterInfo ci, AttackTargetRequest atr, byte error = 1)
        {
            _victimID = victimID;
            _victimHP = victimHP;
            _ci = ci;
            _atr = atr;
            _errorByte = error;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0302;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 23;
            header.Write(bw);

            int attackType = _atr.TargetT == AttackTargetRequest.TargetType.Monster ? 1 : 0;
            int critical = _atr.Critical ? 1 : 0;
            int comboCount = _atr.ComboCount;
            int comboStatus = _atr.ComboStatus;
            byte attackData = (byte)(attackType | (critical << 1) | ((comboCount & 0xF) << 2) | ((comboStatus & 3) << 6));
            byte comboGauge = (byte)(_victimHP == 0 ? 0xC0 : 0);

            bw.Write(_victimID);        // 0Eh
            bw.Write(_victimHP);        // 12h
            bw.Write(_ci.CellIndex);    // 16h
            bw.Write(_atr.Motion);      // 1Ah
            bw.Write((ushort)0);        // 1Ch
            bw.Write(attackData);       // 1Eh
            bw.Write(_ci.CurSP);        // 1Fh
            bw.Write(comboGauge);       // 23h
            bw.Write(_errorByte);       // 24h

            // 1: No error
            // 3: No Equipments For Using the Skill
            // 5: Insufficient Gauge for Using the Skill
            // 6: Wrong Target
            // 7: Unable to use the skill yet
            // 8: Away from the skill use range
            // 13: Skill wlready in use
            // 14: Unable to find target
            // 15: Already dead target
            // 16: the number to attack is short
            // 20: unable to use durring multiple character status
            // 21: the lasting skill was canceled
            // 26: You can use it after certain time passed after attacking
            // 28: If the necessary continuing skill is not learnt, you will be unable to use it
            // 29: The combo sequence does not exist
            // 31: Exceeded maximum multiple characteristic
            // 32: Targeting failed
            // 33: Protected safe mode after moving map
            // 34: Unable to use with duplication
            // 36: You can must select only one of among attack mode or defense mode
        }
    }

    public class PlayerGetAttackedPacket : SendPacketBase     // 0x0303
    {
        Monster _attacker;
        CharacterInfo _ci;
        ushort _motion;
        ushort _attackType;

        public PlayerGetAttackedPacket(Monster attacker, CharacterInfo victim, ushort attackType)
        {
            _attacker = attacker;
            _ci = victim;
            _motion = 1;
            _attackType = attackType;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0303;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 20;
            header.Write(bw);

            bw.Write(_attacker.ID);                     // 0xE
            bw.Write(_attacker.CellIndex);              // 0x12
            bw.Write(_ci.CurHP);                        // 0x16
            bw.Write(_motion);
            bw.Write((ushort)1);
            bw.Write(_attackType);
            bw.Write((byte)(_ci.CurHP > 0 ? 0 : 1));
            bw.Write((byte)1);
        }
    }

    public class ExpGainPacket : SendPacketBase         // 0x0320
    {
        ulong _exp;

        public ExpGainPacket(ulong exp)
        {
            _exp = exp;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0320;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 9;
            header.Write(bw);

            bw.Write(_exp);             // 0xE - Dword - exp gain
            bw.Write((byte)0);          // 0x16 - Byte - gain type?

        }
    }

    public class LevelUpPacket : SendPacketBase         // 0x0321
    {
        CharacterInfo _ci;

        public LevelUpPacket(CharacterInfo ci)
        {
            _ci = ci;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0321;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 54;
            header.Write(bw);

            bw.Write(_ci.Level);
            bw.Write(_ci.MaxHP);
            bw.Write(_ci.MaxSP);
            bw.Write(_ci.MaxMP);
            bw.Write(_ci.Power);
            bw.Write(_ci.Vitality);
            bw.Write(_ci.Sympathy);
            bw.Write(_ci.Intelligence);
            bw.Write(_ci.Stamina);
            bw.Write(_ci.Dexterity);
            bw.Write(_ci.AbilityPoints);
            bw.Write(_ci.AvailableSkillPoints);
            bw.Write(_ci.TotalSkillPoints);
            _ci.WriteAbilityBlock(bw);
            bw.Write(_ci.Exp);
        }
    }

    public class ItemLootPacket : SendPacketBase        // 0x0402
    {
        uint _monsterID;
        Item _item;

        public ItemLootPacket(Item item, uint monsterID)
        {
            _monsterID = monsterID;
            _item = item;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0402;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 22;
            header.Write(bw);

            bw.Write((byte)1);              // 0xE - Error Byte
            _item.Write(bw);
            bw.Write((byte)0);              // Unused spacer byte

            // 0x20 - 112 bytes of some data, starts with npc id?, maybe npc data?
            bw.Write(_monsterID);
            //Utils.WriteZeros(bw, 108);      
        }
    }

    public class GoldLootPacket : SendPacketBase        // 0x0403
    {
        uint _gold;
        uint _victim;

        public GoldLootPacket(uint gold, uint victim)
        {
            _gold = gold;
            _victim = victim;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0403;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 9;
            header.Write(bw);

            bw.Write(_gold);
            bw.Write((byte)1);
            bw.Write(_victim);
        }
    }

    public class UseItemResponse : SendPacketBase                // 0x409
    {
        uint _itemID;
        byte _quantity;
        byte _err;

        public UseItemResponse(uint item, byte newQuantity, Item.ItemError err = Item.ItemError.None)
        {
            _itemID = item;
            _quantity = newQuantity;
            _err = (byte)err;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0409;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 6;
            header.Write(bw);

            // 0xE - Dword - Item id
            // 0x12- Byte - Remaining Quantity
            // 0x13 - Byte - Item Error

            bw.Write(_itemID);
            bw.Write(_quantity);
            bw.Write(_err);
        }
    }

    public class EquipItemResponse : SendPacketBase     // 0x0412 / 0x0413
    {
        CharacterInfo _character;
        Item _newItem;
        Item _oldItem;
        bool _success;

        public EquipItemResponse(CharacterInfo ci, Item newItem, Item oldItem, bool success)
        {
            _character = ci;
            _newItem = newItem;
            _oldItem = oldItem;
            _success = success;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = (ushort)(_oldItem == null ? 0x0412 : 0x0413);
            header.PacketSequenceNumber = sequence;
            header.PacketLength = (ushort)(_oldItem == null ? 46 : 51);
            header.Write(bw);

            if( _oldItem != null )
                bw.Write(_oldItem.ID);
            bw.Write(_newItem.ID);
            if( _oldItem != null )
                bw.Write(_oldItem.Slot);
            bw.Write(_newItem.Slot);
            bw.Write((byte)(_success ? 1 : 0));
            bw.Write((uint)0);
            bw.Write((ushort)0);
            bw.Write(_character.PhysicalDef);
            bw.Write(_character.MagicalDef);
            bw.Write((byte)0);
            bw.Write(_character.AbilityPMin);
            bw.Write(_character.AbilityPMax);
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write(_character.Vitality);
            bw.Write(_character.Sympathy);
            bw.Write(_character.Intelligence);
            bw.Write((ushort)0);
            bw.Write(_character.Dexterity);
            bw.Write(_character.MaxHP);
            bw.Write((uint)0);
            bw.Write(_character.MaxMP);
            bw.Write((byte)0);            
        }
    }

    public class SeeEquipmentChangePacket : SendPacketBase          // 0x0414
    {
        int _characterID;
        Item _item;

        public SeeEquipmentChangePacket(int charID, Item item)
        {
            _characterID = charID;
            _item = item;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0414;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 9;
            header.Write(bw);

            bw.Write(_characterID);
            bw.Write(_item.Model);
            bw.Write((ushort)0);
            bw.Write(_item.Slot);
        }
    }

    public class UnEquipItemResponse : SendPacketBase       // 0x0416
    {
        CharacterInfo _character;
        uint _itemID;
        byte _slot;
        bool _success;

        public UnEquipItemResponse(CharacterInfo ci, uint itemID, byte slot, bool success)
        {
            _character = ci;
            _itemID = itemID;
            _slot = slot;
            _success = success;
        }
        
        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0416;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 46;
            header.Write(bw);
            
            bw.Write(_itemID);
            bw.Write(_slot);
            bw.Write((byte)(_success ? 1 : 2));
            bw.Write((uint)10);
            bw.Write((ushort)20);
            bw.Write(_character.PhysicalDef);
            bw.Write(_character.MagicalDef);
            bw.Write((byte)30);
            bw.Write(_character.AbilityPMin);
            bw.Write(_character.AbilityPMax);
            bw.Write((byte)40);
            bw.Write((byte)50);
            bw.Write(_character.Vitality);
            bw.Write(_character.Sympathy);
            bw.Write(_character.Intelligence);
            bw.Write((ushort)60);
            bw.Write(_character.Dexterity);
            bw.Write(_character.MaxHP);
            bw.Write((uint)70);
            bw.Write(_character.MaxMP);
            bw.Write((byte)80);
        }
    }

    public class SeeUnequipPacket : SendPacketBase  // 0x0417
    {
        int _characterID;
        byte _equipmentSlot;

        public SeeUnequipPacket(int charID, byte equipSlot)
        {
            _characterID = charID;
            _equipmentSlot = equipSlot;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0417;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 5;
            header.Write(bw);

            bw.Write(_characterID);
            bw.Write(_equipmentSlot);
        }
    }

    public class GiveItemPacket : SendPacketBase    // 0x0445
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

    public class SetToolbarLink : SendPacketBase            // 0x0456
    {
        uint _id;
        byte _type;
        byte _err;

        public SetToolbarLink(uint id, ToolbarItemSetRequest.TargetType type, int slot, Item.ItemError err = Item.ItemError.None)
        {
            _id = id;
            int t = (type == ToolbarItemSetRequest.TargetType.Skill ? 3 : 2);
            int typeByte = (slot << 2) | t;
            _type = (byte)typeByte;
            _err = (byte)err;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0456;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 11;
            header.Write(bw);

            bw.Write(_id);              // 0xE - DWord - Skill ID/ItemID
            bw.Write(_type);            // 0x12 - Byte - Type, 2 = Item, 3 = Skill
            bw.Write((uint)0);          // 0x13 - DWord - Secondary ID
            bw.Write((byte)0);          // 0x17 - Byte - Use secondary id, not sure why this is in there, its the same as the first set
            bw.Write(_err);             // 0x18 - Byte - ItemError
        }
    }

    public class ClearToolbarLink : SendPacketBase        // 0x0458
    {
        byte _slot;
        byte _err;

        public ClearToolbarLink(byte slot, Item.ItemError err = Item.ItemError.None)
        {
            _slot = slot;
            _err = (byte)err;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0458;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 2;
            header.Write(bw);

            bw.Write(_slot);
            bw.Write(_err);
        }
    }

    public class GiveQuest : SendPacketBase                // 0x0651
    {
        ushort _wordE;
        ushort _word10;
        byte _byte12;
        uint _dword13;

        public GiveQuest(ushort wordE, ushort word10, byte byte12, uint dword13)
        {
            _wordE = wordE;
            _word10 = word10;
            _byte12 = byte12;
            _dword13 = dword13;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x0651;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 9;
            header.Write(bw);

            bw.Write(_wordE);           // 0x0E - Word - QuestID?
            bw.Write(_word10);          // 0x10 - Word - Some other ID?
            bw.Write(_byte12);          // 0x12 - Byte - 1 = Give Quest?
            bw.Write(_dword13);         // 0x13 - Dword - 
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

    public class NPCDeadPacket : SendPacketBase         // 0x5003
    {
        uint _id;

        public NPCDeadPacket(uint id)
        {
            _id = id;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x5003;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 4;
            header.Write(bw);

            bw.Write(_id);
        }
    }

    public class NPCMovePacket : SendPacketBase         // 0x5011
    {
        DecoServer2.NPC _npc;

        public NPCMovePacket(DecoServer2.NPC npc)
        {
            _npc = npc;
        }

        public override void Write(uint sequence, BinaryWriter bw)
        {
            PacketHeader header = new PacketHeader();
            header.Opcode = 0x5011;
            header.PacketSequenceNumber = sequence;
            header.PacketLength = 10;
            header.Write(bw);

            bw.Write(_npc.ID);
            bw.Write(_npc.MoveSpeed);
            bw.Write((byte)1);
            bw.Write(_npc.CellIndex);

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
