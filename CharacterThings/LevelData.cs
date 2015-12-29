using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2
{
    public class LevelData
    {
        uint _level;
        ulong _exp;
        uint _maxHP;
        uint _maxMP;
        uint _maxSP;
        uint _power;
        uint _vitality;
        uint _sympathy;
        uint _intelligence;
        uint _stamina;
        uint _dexterity;
        uint _abilityPoints;
        uint _skillPoints;
        uint _moveSpeed;
        uint _abilityPMin;
        uint _abilityPMax;
        uint _pyhsicalDef;
        uint _magicalDef;
        uint _attackSpeed;
        uint _abilityMMin;
        uint _abilityMMax;

        public uint Level { get { return _level; } }
        public ulong Exp { get { return _exp; } }
        public uint MaxHP { get { return _maxHP; } }
        public uint MaxMP { get { return _maxMP; } }
        public uint MaxSP { get { return _maxSP; } }
        public uint Power { get { return _power; } }
        public uint Vitality { get { return _vitality; } }
        public uint Sympathy { get { return _sympathy; } }
        public uint Intelligence { get { return _intelligence; } }
        public uint Stamina { get { return _stamina; } }
        public uint Dexterity { get { return _dexterity; } }
        public uint AbilityPoints { get { return _abilityPoints; } }
        public uint SkillPoints { get { return _skillPoints; } }
        public uint MoveSpeed { get { return _moveSpeed; } }
        public uint AbilityPMin { get { return _abilityPMin; } }
        public uint AbilityPMax { get { return _abilityPMax; } }
        public uint PhysicalDef { get { return _pyhsicalDef; } }
        public uint MagicalDef { get { return _magicalDef; } }
        public uint AttackSpeed { get { return _attackSpeed; } }
        public uint AbilityMMin { get { return _abilityMMin; } }
        public uint AbilityMMax { get { return _abilityMMax; } }

        public static LevelData ReadFromDB(object[] row)
        {
            // 0: level   int(10) unsigned
            // 1: exp_req bigint(20) unsigned
            // 2: max_hp  int(10) unsigned
            // 3: max_mp  int(10) unsigned
            // 4: max_sp  int(10) unsigned
            // 5: power   int(10) unsigned
            // 6: vitality    int(10) unsigned
            // 7: sympathy    int(10) unsigned
            // 8: intelligence    int(10) unsigned
            // 9: stamina int(10) unsigned
            // 10: dexterity   int(10) unsigned
            // 11: ability_points  int(10) unsigned
            // 12: skill_points    int(10) unsigned
            // 13: move_speed  int(10) unsigned
            // 14: ability_pmin    int(10) unsigned
            // 15: ability_pmax    int(10) unsigned
            // 16: physical_def    int(10) unsigned
            // 17: magical_def int(10) unsigned
            // 18: attack_speed    int(10) unsigned
            // 19: ability_mmin    int(10) unsigned
            // 20: ability_mmax    int(10) unsigned

            LevelData ld = new LevelData();
            ld._level = (uint)row[0];
            ld._exp = (ulong)row[1];
            ld._maxHP = (uint)row[2];
            ld._maxMP = (uint)row[3];
            ld._maxSP = (uint)row[4];
            ld._power = (uint)row[5];
            ld._vitality = (uint)row[6];
            ld._sympathy = (uint)row[7];
            ld._intelligence = (uint)row[8];
            ld._stamina = (uint)row[9];
            ld._dexterity = (uint)row[10];
            ld._abilityPoints = (uint)row[11];
            ld._skillPoints = (uint)row[12];
            ld._moveSpeed = (uint)row[13];
            ld._abilityPMin = (uint)row[14];
            ld._abilityPMax = (uint)row[15];
            ld._pyhsicalDef = (uint)row[16];
            ld._magicalDef = (uint)row[17];
            ld._attackSpeed = (uint)row[18];
            ld._abilityMMin = (uint)row[19];
            ld._abilityMMax = (uint)row[20];
            return ld;
        }
    }
}
