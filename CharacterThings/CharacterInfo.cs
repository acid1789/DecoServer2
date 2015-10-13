﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JuggleServerCore;

namespace DecoServer2.CharacterThings
{
    public class CharacterInfo
    {
        const int StartingHP = 150;
        const int StartingSP = 150;
        const int StartingMP = 150;


        CharacterSelectInfo _csi;

        uint _worldID;

        ushort _mapID;
        uint _cellIndex;
        uint _curHP;
        uint _curSP;
        uint _curMP;
        uint _gold;
        uint _pvpWins;
        uint _pvpCount;
        ulong _exp;

        uint _fame;
        uint _nationRate;

        ushort _moveSpeed;
        ushort _abilityPMin;
        ushort _abilityPMax;
        ushort _physicalDef;
        ushort _magicalDef;
        byte _attackSpeed;
        ushort _abilityMMin;
        ushort _abilityMMax;

        uint _maxHP;
        uint _maxSP;
        uint _maxMP;

        ushort _power;
        ushort _vitality;
        ushort _sympathy;
        ushort _intelligence;
        ushort _stamina;
        ushort _dexterity;
        byte _charisma;
        byte _luck;

        ushort _abilityPoints;
        ushort _leftSP;
        ushort _totalSP;

        List<Item> _clothes;
        List<Item> _generalItems;
        List<Item> _items;
        List<Item> _questItems;
        List<ushort> _skills;
        List<ushort> _buffs = new List<ushort>();
        List<Booster> _boosters = new List<Booster>();
        List<Item> _ridingItems;

        int _frontierID;
        byte _frontierLevel;
        string _frontierString = "";
        int _frontierMembershipFee;

        bool _loadingData;
        bool _expectingHV;
        bool _expectingLV;
        bool _expectingItems;
        bool _expectingFrontier;
        bool _expectingSkills;

        public CharacterInfo(CharacterSelectInfo csi)
        {
            _csi = csi;

            //_buffs.Add(125);   
            //_boosters.Add(new Booster(11114, 500));
            //_boosters.Add(new Booster(11115, 500));
            //_boosters.Add(new Booster(11116, 500));
        }

        public void ReadHV(DBQuery query)
        {
            // 0: character_id
            // 1: map_id
            // 2: cell_index
            // 3: hp
            // 4: sp
            // 5: mp
            // 6: gold
            // 7: pvp_wins
            // 8: pvp_count
            // 9: exp
            object[] row = query.Rows[0];

            _mapID = (ushort)row[1];
            _cellIndex = (uint)row[2];
            _curHP = (uint)row[3];
            _curSP = (uint)row[4];
            _curMP = (uint)row[5];
            _gold = (uint)row[6];
            _pvpWins = (uint)row[7];
            _pvpCount = (uint)row[8];
            _exp = (ulong)row[9];

            _expectingHV = false;
        }

        public string SetHVDefaults()
        {
            _mapID = (ushort)(_csi.Millena ? 5 : 7);    // 5 for millena, 7 for rain
            _cellIndex = _csi.Millena ? Utils.EncodeCellIndex(_mapID, 135, 52) : Utils.EncodeCellIndex(_mapID, 221, 222);
            _curHP = _maxHP = 150;
            _curSP = _maxSP = 150;
            _curMP = _maxMP = 150;
            _gold = 123;
            _pvpWins = _pvpCount = 0;
            _exp = 0;

            _expectingHV = false;
            return HVString;
        }

        public void ReadLV(DBQuery query)
        {
            // 0: character_id      int(10) unsigned
            // 1: fame              int(10) unsigned
            // 2: nation_rate       int(10) unsigned
            // 3: move_speed        smallint(5) unsigned
            // 4: ability_p_min     smallint(5) unsigned
            // 5: ability_p_max     smallint(5) unsigned
            // 6: attack_speed      tinyint(3) unsigned
            // 7: ability_m_min     smallint(5) unsigned
            // 8: ability_m_max     smallint(5) unsigned
            // 9: hp                int(10) unsigned
            // 10: sp               int(10) unsigned
            // 11: mp               int(10) unsigned
            // 12: magical_def      smallint(5) unsigned
            // 13: physical_def     smallint(5) unsigned
            // 14: power            smallint(5) unsigned
            // 15: vitality         smallint(5) unsigned
            // 16: sympathy         smallint(5) unsigned
            // 17: intelligence     smallint(5) unsigned
            // 18: stamina          smallint(5) unsigned
            // 19: dexterity        smallint(5) unsigned
            // 20: charisma         smallint(5) unsigned
            // 21: luck             tinyint(3) unsigned
            // 22: ability_points   smallint(5) unsigned
            // 23: left_sp          smallint(5) unsigned
            // 24: total_sp         smallint(5) unsigned
            // 25: frontier_id      int(10)

            object[] row = query.Rows[0];
            _fame = (uint)row[1];
            _nationRate = (uint)row[2];
            _moveSpeed = (ushort)row[3];
            _abilityPMin = (ushort)row[4];
            _abilityPMax = (ushort)row[5];
            _attackSpeed = (byte)row[6];
            _abilityMMin = (ushort)row[7];
            _abilityMMax = (ushort)row[8];
            _maxHP = (uint)row[9];
            _maxSP = (uint)row[10];
            _maxMP = (uint)row[11];
            _magicalDef = (ushort)row[12];
            _physicalDef = (ushort)row[13];
            _power = (ushort)row[14];
            _vitality = (ushort)row[15];
            _sympathy = (ushort)row[16];
            _intelligence = (ushort)row[17];
            _stamina = (ushort)row[18];
            _dexterity = (ushort)row[19];
            _charisma = (byte)row[20];
            _luck = (byte)row[21];
            _abilityPoints = (ushort)row[22];
            _leftSP = (ushort)row[23];
            _totalSP = (ushort)row[24];
            _frontierID = (int)row[25];

            _expectingLV = false;
        }

        public string SetLVDefaults()
        {
            _fame = 0;
            _nationRate = 0;
            _moveSpeed = 192;
            _abilityPMin = _abilityMMin = 10;
            _abilityPMax = _abilityMMax = 12;
            _attackSpeed = 8;
            _maxHP = StartingHP;
            _maxSP = StartingSP;
            _maxMP = StartingMP;
            _magicalDef = (ushort)(_csi.Millena ? 2 : 5);
            _physicalDef = (ushort)(_csi.Millena ? 5 : 2);
            _power = 13;
            _vitality = 9;
            _sympathy = 13;
            _intelligence = 12;
            _stamina = 12;
            _dexterity = 13;
            _charisma = 0;
            _luck = 0;
            _abilityPoints = 0;
            _leftSP = 0;
            _totalSP = 100;
            _frontierID = 0;

            _expectingLV = false;

            return LVString;
        }

        public void ReadItems(DBQuery query)
        {
            _clothes = new List<Item>();
            _generalItems = new List<Item>();
            _items = new List<Item>();
            _questItems = new List<Item>();
            _ridingItems = new List<Item>();

            foreach (object[] row in query.Rows)
            {
                Item item = Item.ReadFromDB(row);
                int index = -1;
                switch (item.ItemType)
                {
                    case Item.Type.Clothing:
                        index = _clothes.Count;
                        _clothes.Add(item);
                        break;
                    case Item.Type.General:
                        index = _generalItems.Count;
                        _generalItems.Add(item);
                        break;
                    case Item.Type.Item:
                        index = _items.Count;
                        _items.Add(item);
                        break;
                    case Item.Type.Quest:
                        index = _questItems.Count;
                        _questItems.Add(item);
                        break;
                    case Item.Type.Riding:
                        index = _ridingItems.Count;
                        _ridingItems.Add(item);
                        break;
                }
                item.Slot = (byte)index;
            }

            _expectingItems = false;
        }

        public void ReadFrontierData(DBQuery query)
        {
            // 0: frontier_id int(10) unsigned
            // 1: name    varchar(32)
            // 2: level   tinyint(3) unsigned
            // 3: membership_fee  int(10) unsigned

            if (query.Rows.Count > 0)
            {
                object[] row = query.Rows[0];
                _frontierString = (string)row[1];
                _frontierLevel = (byte)row[2];
                _frontierMembershipFee = (int)row[3];
            }
            _expectingFrontier = false;
        }

        public void ReadSkills(DBQuery query)
        {
            // 0: character_id
            // 1: skill_id
            _skills = new List<ushort>();
            foreach (object[] row in query.Rows)
            {
                ushort skill = (ushort)row[1];
                _skills.Add(skill);
            }
            _expectingSkills = false;
        }

        public void WritePacket(BinaryWriter bw)
        {
            Utils.WriteByteString(bw, _csi.Name, 17);
            Utils.WriteZeros(bw, 31);       // Unknown string
            bw.Write(_csi.ModelInfo);
            bw.Write(_csi.Job);

            bw.Write((byte)0);              // Unknown Byte

            bw.Write(_exp);
            bw.Write(_csi.Level);

            bw.Write((uint)1);              // Unknowns
            bw.Write((byte)1);

            bw.Write(_fame);
            bw.Write(_nationRate);
            bw.Write(_mapID);
            bw.Write(_cellIndex);
            bw.Write(_curHP);
            bw.Write(_curSP);
            bw.Write(_curMP);

            bw.Write((byte)0);              // Unknown byte

            bw.Write(_moveSpeed);
            bw.Write(_abilityPMin);
            bw.Write(_abilityPMax);
            bw.Write(_physicalDef);
            bw.Write(_magicalDef);
            bw.Write(_attackSpeed);
            bw.Write(_abilityMMin);
            bw.Write(_abilityMMax);

            bw.Write(_maxHP);
            bw.Write(_maxSP);
            bw.Write(_maxMP);

            bw.Write(_power);
            bw.Write(_vitality);
            bw.Write(_sympathy);
            bw.Write(_intelligence);
            bw.Write(_stamina);
            bw.Write(_dexterity);
            bw.Write(_charisma);
            bw.Write(_luck);

            bw.Write(_abilityPoints);
            bw.Write(_leftSP);
            bw.Write(_totalSP);
            bw.Write(_pvpWins);
            bw.Write(_pvpCount);
            bw.Write(_gold);

            bw.Write((ushort)0);            // Unknown short

            int itemCounts = (_clothes.Count & 0x1F) | ((_generalItems.Count & 0x3F) << 5) | ((_items.Count & 0x3F) << 11) | ((_questItems.Count & 0x1F) << 17) | ((_skills.Count & 0x7F) << 22);
            bw.Write(itemCounts);
            bw.Write((byte)_buffs.Count);

            int unknownCounts = 0;
            bw.Write(unknownCounts);        // Bitfield of unknown counts

            bw.Write((byte)_boosters.Count);
            bw.Write((byte)_ridingItems.Count);

            bw.Write((byte)0);              // Unknown count            
            for (int i = 0; i < 7; i++)    // more unknown bytes
                bw.Write((byte)1);

            foreach (Item item in _clothes)
                item.Write(bw);

            foreach (Item item in _generalItems)
                item.Write(bw);

            foreach (Item item in _items)
                item.Write(bw);

            foreach (Item item in _questItems)
                item.Write(bw);

            foreach (ushort skill in _skills)
                bw.Write(skill);

            foreach (ushort buff in _buffs)
                bw.Write(buff);

            foreach (Booster booster in _boosters)
                booster.Write(bw);

            // frontier data
            bw.Write((uint)1);                  // Unknown int - frontier id?
            bw.Write(_frontierLevel);           // Frontier level
            Utils.WriteByteString(bw, _frontierString, 15);
            bw.Write(_frontierMembershipFee);

            foreach (Item item in _ridingItems)
                item.Write(bw);

            Utils.WriteZeros(bw, 14);       // Unknown 14 bytes
        }


        #region Accessors
        public bool LoadComplete
        {
            get
            {
                bool complete = true;
                if (_loadingData)
                {
                    complete = !_expectingHV && !_expectingLV && !_expectingItems && !_expectingFrontier && !_expectingSkills;
                    if (complete)
                        _loadingData = false;
                }
                return complete;
            }
        }

        public bool ExpectingHV
        {
            get { return _expectingHV; }
            set { _expectingHV = value; if (value) _loadingData = true; }
        }

        public bool ExpectingLV
        {
            get { return _expectingLV; }
            set { _expectingLV = value; if (value) _loadingData = true; }
        }

        public bool ExpectingItems
        {
            get { return _expectingItems; }
            set { _expectingItems = value; if (value) _loadingData = true; }
        }

        public bool ExpectingFrontier
        {
            get { return _expectingFrontier; }
            set { _expectingFrontier = value; if (value) _loadingData = true; }
        }

        public bool ExpectingSkills
        {
            get { return _expectingSkills; }
            set { _expectingSkills = value; if (value) _loadingData = true; }
        }

        public string HVString
        {
            get { return string.Format("INSERT INTO characters_hv SET character_id={0},map_id={1},cell_index={2},hp={3},sp={4},mp={5},gold={6},pvp_wins={7},pvp_count={8},exp={9}", _csi.ID, _mapID, _cellIndex, _curHP, _curSP, _curMP, _gold, _pvpWins, _pvpCount, _exp); }
        }

        public string LVString
        {
            get
            {
                return string.Format(@"INSERT INTO characters_lv SET character_id={0},fame={1},nation_rate={2},move_speed={3},ability_p_min={4},ability_p_max={5},attack_speed={6},ability_m_min={7},ability_m_max={8},
                                                                    hp={9},sp={10},mp={11},magical_def={12},physical_def={13},power={14},vitality={15},sympathy={16},intelligence={17},stamina={18},dexterity={19},charisma={20},
                                                                    luck={21},ability_points={22},left_sp={23},total_sp={24},frontier_id={25};",
                                                                    _csi.ID, _fame, _nationRate, _moveSpeed, _abilityPMin, _abilityPMax, _attackSpeed, _abilityMMin, _abilityMMax, _maxHP, _maxSP, _maxMP, _magicalDef, _physicalDef, _power, _vitality, _sympathy, _intelligence, _stamina, _dexterity, _charisma, _luck, _abilityPoints, _leftSP, _totalSP, _frontierID);
            }

        }

        public int FrontierID
        {
            get { return _frontierID; }
        }

        public ushort DataSize
        {
            get
            {
                int size = 175;
                size += 16 * _clothes.Count;
                size += 16 * _generalItems.Count;
                size += 16 * _items.Count;
                size += 16 * _questItems.Count;
                size += 2 * _skills.Count;
                size += 2 * _buffs.Count;
                size += 6 * _boosters.Count;
                size += 24; // frontier data
                size += 16 * _ridingItems.Count;

                size += 14; // end bytes
                return (ushort)size;
            }
        }

        public int ID
        {
            get { return _csi.ID; }
        }

        public uint WorldID
        {
            get { return _worldID; }
            set { _worldID = value; }
        }

        public ushort MapID
        {
            get { return _mapID; }
        }

        public ushort MoveSpeed
        {
            get { return _moveSpeed; }
        }

        public uint CellIndex
        {
            get { return _cellIndex; }
            set { _cellIndex = value; }
        }
        #endregion

    }
}