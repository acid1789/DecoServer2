using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JuggleServerCore;
using DecoServer2.Quests;

namespace DecoServer2
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

        Dictionary<byte, Item> _equipped;
        Dictionary<uint, Item> _generalItems;
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

        Dictionary<uint, byte> _activeQuests;
        List<uint> _completedQuests;

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
            // 10: fame              int(10) unsigned
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
            _fame = (uint)row[10];

            _curHP = 150;

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
            _fame = 0;

            _expectingHV = false;
            return HVString;
        }

        public void ReadLV(DBQuery query)
        {
            // 0: character_id      int(10) unsigned
            // 1: nation_rate       int(10) unsigned
            // 2: move_speed        smallint(5) unsigned
            // 3: ability_p_min     smallint(5) unsigned
            // 4: ability_p_max     smallint(5) unsigned
            // 5: attack_speed      tinyint(3) unsigned
            // 6: ability_m_min     smallint(5) unsigned
            // 7: ability_m_max     smallint(5) unsigned
            // 8: hp                int(10) unsigned
            // 9: sp               int(10) unsigned
            // 10: mp               int(10) unsigned
            // 11: magical_def      smallint(5) unsigned
            // 12: physical_def     smallint(5) unsigned
            // 13: power            smallint(5) unsigned
            // 14: vitality         smallint(5) unsigned
            // 15: sympathy         smallint(5) unsigned
            // 16: intelligence     smallint(5) unsigned
            // 17: stamina          smallint(5) unsigned
            // 18: dexterity        smallint(5) unsigned
            // 19: charisma         smallint(5) unsigned
            // 20: luck             tinyint(3) unsigned
            // 21: ability_points   smallint(5) unsigned
            // 22: left_sp          smallint(5) unsigned
            // 23: total_sp         smallint(5) unsigned
            // 24: frontier_id      int(10)

            object[] row = query.Rows[0];
            _nationRate = (uint)row[1];
            _moveSpeed = (ushort)row[2];
            _abilityPMin = (ushort)row[3];
            _abilityPMax = (ushort)row[4];
            _attackSpeed = (byte)row[5];
            _abilityMMin = (ushort)row[6];
            _abilityMMax = (ushort)row[7];
            _maxHP = (uint)row[8];
            _maxSP = (uint)row[9];
            _maxMP = (uint)row[10];
            _magicalDef = (ushort)row[11];
            _physicalDef = (ushort)row[12];
            _power = (ushort)row[13];
            _vitality = (ushort)row[14];
            _sympathy = (ushort)row[15];
            _intelligence = (ushort)row[16];
            _stamina = (ushort)row[17];
            _dexterity = (ushort)row[18];
            _charisma = (byte)row[19];
            _luck = (byte)row[20];
            _abilityPoints = (ushort)row[21];
            _leftSP = (ushort)row[22];
            _totalSP = (ushort)row[23];
            _frontierID = (int)row[24];

            _expectingLV = false;
        }

        public string SetLVDefaults()
        {
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
            _equipped = new Dictionary<byte, Item>();
            _generalItems = new Dictionary<uint, Item>();
            _items = new List<Item>();
            _questItems = new List<Item>();
            _ridingItems = new List<Item>();

            foreach (object[] row in query.Rows)
            {
                Item item = Item.ReadFromDB(row);
                AddItem(item);
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

        public void ReadActiveQuests(DBQuery query, Connection client)
        {   
            _activeQuests = new Dictionary<uint, byte>();
            foreach (object[] row in query.Rows)
            {
                // 0: character_id	    int(10) unsigned
                // 1: quest_id          int(11) unsigned
                // 2: step              tinyint(3) unsigned
                uint quest = (uint)row[1];
                byte step = (byte)row[2];
                _activeQuests[quest] = step;

                Quest q = Program.Server.GetQuest(quest);
                QuestStep qs = q.GetStep(step);
                qs.Activate(client);
            }
        }

        public void ReadCompletedQuests(DBQuery query)
        {
            _completedQuests = new List<uint>();
            foreach (object[] row in query.Rows)
            {
                // 0: character_id      int(10) unsigned
                // 1: quest_id          int(10) unsigned
                uint quest = (uint)row[1];
                _completedQuests.Add(quest);
            }
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

            //int itemCounts = (_equipped.Count & 0x1F) | ((_generalItems.Count & 0x3F) << 5) | ((_items.Count & 0x3F) << 11) | ((_questItems.Count & 0x1F) << 17) | ((_skills.Count & 0x7F) << 22);
            int itemCounts = (_equipped.Count & 0x1F) | ((_items.Count & 0x3F) << 5) | ((_generalItems.Count & 0x3F) << 11) | ((_questItems.Count & 0x1F) << 17) | ((_skills.Count & 0x7F) << 22);
            bw.Write(itemCounts);
            bw.Write((byte)_buffs.Count);

            int unknownCounts = 0;
            bw.Write(unknownCounts);        // Bitfield of unknown counts

            bw.Write((byte)_boosters.Count);
            bw.Write((byte)_ridingItems.Count);
            
            foreach (Item item in _equipped.Values)
                item.Write(bw);
            
            foreach (Item item in _items)
                item.Write(bw);

            foreach (Item item in _generalItems.Values)
                item.Write(bw);

            foreach (Item item in _questItems)
                item.Write(bw);

            foreach (ushort skill in _skills)
                bw.Write(skill);

            // Write the mysterious 6 bytes
            bw.Write((uint)1);
            bw.Write((ushort)1);

            Utils.WriteByteString(bw, "unk70Bytes", 70);

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

            Utils.WriteZeros(bw, 14);       // Extra padding workaround for client bug
        }

        public void GainExp(int exp)
        {
            if( exp >= 0 )
                _exp += (ulong)exp;
        }

        public void GainGold(int gold)
        {
            if( gold >= 0 )
                _gold += (uint)gold;
        }

        #region Quest Area
        public bool HasActiveQuest(uint questID)
        {
            return _activeQuests.ContainsKey(questID);
        }

        public bool HasCompletedQuest(uint questID)
        {
            return _completedQuests.Contains(questID);
        }

        public byte GetQuestStep(uint questID)
        {
            return _activeQuests[questID];
        }

        public Quests.Quest GetActiveQuestForNPC(uint npcID)
        {
            foreach (KeyValuePair<uint, byte> aq in _activeQuests)
            {
                Quest q = Program.Server.GetQuest(aq.Key);
                QuestStep qs = q.GetStep(aq.Value);
                if( qs.IsNPCRelevent(npcID) )
                    return q;
            }
            return null;
        }

        public void ReceiveQuest(Quests.Quest q)
        {
            SetActiveQuestStep(q.QuestID, 0);
        }

        public void SetActiveQuestStep(uint questID, byte step)
        {
            _activeQuests[questID] = step;
            Program.Server.TaskProcessor.AddTask(new JuggleServerCore.Task(JuggleServerCore.Task.TaskType.CharacterActiveQuest_Save, null, new ActiveQuestArgs(ID, questID, step)));
        }

        public void CompleteQuest(uint questID)
        {
            _activeQuests.Remove(questID);
            _completedQuests.Add(questID);
            Program.Server.TaskProcessor.AddTask(new JuggleServerCore.Task(JuggleServerCore.Task.TaskType.CharacterActiveQuest_Save, null, new ActiveQuestArgs(ID, questID, 0, true, true)));
        }
        #endregion

        public bool HasItem(uint templateID)
        {
            List<Item> allItems = new List<Item>();
            allItems.AddRange(_equipped.Values);
            allItems.AddRange(_generalItems.Values);
            allItems.AddRange(_items);
            allItems.AddRange(_questItems);
            allItems.AddRange(_ridingItems);

            foreach (Item i in allItems)
            {
                if( i.TemplateID == templateID)
                    return true;
            }

            return false;
        }

        public bool HasItemEquipped(uint templateID)
        {
            foreach (Item i in _equipped.Values)
            {
                if( i.TemplateID == templateID )
                    return true;
            }
            return false;
        }

        public Item FindItem(uint itemID)
        {
            if( _generalItems.ContainsKey(itemID) )
                return _generalItems[itemID];
            return null;
        }

        public void AddItem(Item item)
        {
            switch (item.ItemType)
            {
                case Item.Type.Equipped:
                    _equipped[item.Slot] = item;
                    break;
                case Item.Type.General:
                    _generalItems[item.ID] = item;
                    break;
                case Item.Type.Item:
                    break;
                case Item.Type.Quest:                    
                    break;
                case Item.Type.Riding:
                    break;
            }

            if (item.Slot == 0xFF && item.ItemType == Item.Type.General)
            {
                item.Slot = FindGeneralSlot();                
            }
        }

        public byte FindGeneralSlot()
        {
            // Find a free slot for this item
            byte slot = 0;
            while (true)
            {
                bool valid = true;
                foreach (Item i in _generalItems.Values)
                {
                    if (i.Slot == slot)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    return slot;
                }
                slot++;
                if (slot > 18)
                    break;
            }
            return 0xFF;
        }

        public Item EquippedItem(byte slot)
        {
            if( _equipped.ContainsKey(slot) )
                return _equipped[slot];
            return null;
        }

        public Item EquippedItem(uint id)
        {
            foreach (Item i in _equipped.Values)
            {
                if( i.ID == id )
                    return i;
            }
            return null;
        }

        public void UnEquipItem(byte slot, byte targetSlot = 0xFF)
        {
            if (_equipped.ContainsKey(slot))
            {
                Item equipped = _equipped[slot];
                _equipped.Remove(slot);
                equipped.Slot = targetSlot;
                equipped.ItemType = Item.Type.General;
                AddItem(equipped);
            }
        }

        public void EquipItem(Item item, byte slot)
        {
            if( _equipped.ContainsKey(slot) )
                UnEquipItem(slot);

            _equipped[slot] = item;
            item.Slot = slot;
            item.ItemType = Item.Type.Equipped;
            _generalItems.Remove(item.ID);
        }

        public void AddGoldExpFame(uint gold, uint exp, uint fame)
        {
            _gold += gold;
            _exp += exp;
            _fame += fame;
        }

        public void Teleport(Location loc)
        {
            _mapID = loc.Map;
            _cellIndex = loc.CellIndex;
        }

        public void AttackTarget(Monster m, AttackTargetRequest atr)
        {
            bool millena = Millena;
            int damage = Program.Server.Rand.Next(millena ? _abilityPMin : _abilityMMin, millena ? _abilityPMax : _abilityMMax);

            bool critical = (Program.Server.Rand.NextDouble() < 0.25);
            if( critical )
                damage *= 2;
            atr.Critical = critical;

            m.TakeDamage((uint)damage, ID);
        }

        public void TakeDamage(int damage)
        {
            /*
            if (damage < 0)
                _curHP += (uint)(-damage);
            else
            {
                uint dmg = (uint)damage;
                if( dmg < _physicalDef )
                    dmg = 0;
                else
                    dmg -= _physicalDef;
                if ( dmg > _curHP )
                    _curHP = 0;
                else
                    _curHP -= dmg;
            }
            */
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
            get
            {
                return string.Format("INSERT INTO characters_hv (character_id,map_id,cell_index,hp,sp,mp,gold,pvp_wins,pvp_count,exp,fame) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) ON DUPLICATE KEY UPDATE map_id=VALUES(map_id),cell_index=VALUES(cell_index),hp=VALUES(hp),sp=VALUES(sp),mp=VALUES(mp),gold=VALUES(gold),pvp_wins=VALUES(pvp_wins),pvp_count=VALUES(pvp_count),exp=VALUES(exp),fame=VALUES(fame);", _csi.ID, _mapID, _cellIndex, _curHP, _curSP, _curMP, _gold, _pvpWins, _pvpCount, _exp, _fame);
            }
        }

        public string LVString
        {
            get
            {
                return string.Format(@"INSERT INTO characters_lv SET character_id={0},nation_rate={2},move_speed={3},ability_p_min={4},ability_p_max={5},attack_speed={6},ability_m_min={7},ability_m_max={8},
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
                int size = 167;
                size += 16 * _equipped.Count;
                size += 16 * _generalItems.Count;
                size += 16 * _items.Count;
                size += 16 * _questItems.Count;

                size += 2 * _skills.Count;

                size += 6;
                size += 70;

                size += 2 * _buffs.Count;
                size += 6 * _boosters.Count;

                size += 24; // frontier data

                size += 16 * _ridingItems.Count;

                size += 14; // Extra junk data for packet header offset.  Stupid bug in the client code
                
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
            set
            {
                _cellIndex = value;
                // From here, save in the database
                Program.Server.TaskProcessor.AddDBQuery(HVString, null, false);
            }
        }

        public byte Level
        {
            get { return _csi.Level; }
        }

        public bool Millena
        {
            get { return _csi.Millena; }
        }

        public bool Male
        {
            get { return _csi.Male; }
        }

        public byte Job
        {
            get { return _csi.Job; }
        }

        public uint Fame
        {
            get { return _fame; }
        }

        public ulong Exp
        {
            get { return _exp; }
        }

        public uint Gold
        {
            get { return _gold; }
        }

        public ushort PhysicalDef
        {
            get { return _physicalDef; }
        }

        public ushort MagicalDef
        {
            get { return _magicalDef; }
        }

        public ushort AbilityPMin
        {
            get { return _abilityPMin; }        
        }

        public ushort AbilityPMax
        {
            get { return _abilityPMax; }
        }

        public ushort Vitality
        {
            get { return _vitality; }
        }

        public ushort Sympathy
        {
            get { return _sympathy; }
        }

        public ushort Intelligence
        {
            get { return _intelligence; }
        }

        public ushort Dexterity
        {
            get { return _dexterity; }
        }

        public uint MaxHP
        {
            get { return _maxHP; }
        }

        public uint MaxMP
        {
            get { return _maxHP; }
        }

        public uint MaxSP
        {
            get { return _maxSP; }
        }

        public uint CurHP
        {
            get { return _curHP; }
        }

        public uint CurMP
        {
            get { return _curMP; }
        }

        public uint CurSP
        {
            get { return _curSP; }
        }
        #endregion

    }
    
    public class ActiveQuestArgs
    {
        public int CharacterID;
        public uint QuestID;
        public byte Step;
        public bool Remove;
        public bool Finished;

        public ActiveQuestArgs(int characterID, uint questID, byte step, bool remove = false, bool finished = false)
        {
            CharacterID = characterID;
            QuestID = questID;
            Step = step;
            Remove = remove;
            Finished = finished;
        }
    }

    public class GiveGoldExpFameArgs
    {
        public enum TheReason
        {
            Quest
        }

        public uint Gold;
        public uint Fame;
        public uint Exp;
        public TheReason Reason;
        public uint Context;

        public GiveGoldExpFameArgs(uint gold, uint exp, uint fame, TheReason reason, uint context)
        {
            Gold = gold;
            Exp = exp;
            Fame = fame;
            Reason = reason;
            Context = context;
        }
    }

    public class GiveItemArgs
    {
        public enum TheReason
        {
            Quest,
            Loot,
            GMCommand
        }

        public uint ItemTemplateID;
        public TheReason Reason;
        public uint Context;
        public Item Item;

        public GiveItemArgs(uint itemTemplateID, TheReason reason, uint context)
        {
            ItemTemplateID = itemTemplateID;
            Reason = reason;
            Context = context;
        }
    }
}
