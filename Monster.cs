using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using JuggleServerCore;

namespace DecoServer2
{
    public class Monster : NPC
    {
        enum AIState
        {
            Idle,
            Moving,
            Attack,
            Dead,
        };

        MonsterTemplate _template;
        Location _loc;

        AIState _aiState;
        AIState _aiNextState;
        double _aiTimer;
        Vector _moveTarget;
        float _moveSpeed;


        static uint s_monsterID = 0xFFFF;
        public Monster(Location loc, MonsterTemplate template) : base()
        {
            s_monsterID++;
            if( s_monsterID < 0xFFFF )
                s_monsterID = 0xFFFF + 1;

            _loc = loc;
            _template = template;

            _id = s_monsterID;
            _gameID = (ushort)_template.GameID;
            _cellIndex = loc.RandomCell;
            _mapID = loc.Map;
            _hp = _template.HP;
            _aiState = AIState.Idle;
            _aiTimer = 0;
        }

        public void Update(double deltaSeconds)
        {
            if (_aiTimer > 0)
                _aiTimer -= deltaSeconds;

            switch (_aiState)
            {
                case AIState.Idle:
                    if (_aiTimer <= 0)
                    {
                        if (Program.Server.Rand.NextDouble() < _template.IdleMoveChance)
                        {
                            // Wander to a new spot
                            _moveTarget = Utils.DecodeCellIndex(_loc.Map, _loc.RandomCell);
                            _moveSpeed = _template.IdleMoveSpeed;
                            _aiState = AIState.Moving;
                            _aiNextState = AIState.Idle;
                        }
                        else
                            _aiTimer = 1;
                    }
                    break;
                case AIState.Moving:
                    if (_aiTimer <= 0)
                    {
                        // Move along the vector
                        Vector pos = Utils.DecodeCellIndex(_loc.Map, _cellIndex);
                        Vector delta = _moveTarget - pos;
                        double dist = delta.Length;
                        if (dist > _moveSpeed)
                            dist = _moveSpeed;
                        if (dist < 1)
                        {
                            // done moving
                            _aiState = _aiNextState;
                        }
                        else
                        {
                            delta.Normalize();
                            Vector target = pos + (delta * dist);

                            _cellIndex = Utils.EncodeCellIndex(_loc.Map, (uint)target.X, (uint)target.Y);
                            Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.UpdateNPCPosition, null, this));
                        }                        
                        _aiTimer = 1;
                    }
                    break;
                case AIState.Attack:
                    break;
                case AIState.Dead:
                    break;
            }
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            /*
            // 0xE - uint
            // 0x12 - ushort
            // 0x14 - uint
            // 0x18 - uint
            // 0x1C - byte
            // 0x1D - ushort
            // 0x1F - byte
            // 0x20 - byte
            // 0x21 - uint
            // 0x25 - uint
            // 0x29 - ??
            // 0x2D - byte[33]
            // 0x4E - uint


            bw.Write(_id);                              
            bw.Write(_gameID);                          
            bw.Write(_cellIndex);                       
            bw.Write(_hp);
            //bw.Write((byte)1);
            //bw.Write((ushort)1);
            //bw.Write((byte)1);
            //bw.Write((byte)1);
            //bw.Write((uint)0);
            //bw.Write((uint)0);
            //bw.Write((uint)0);
            //Utils.WriteZeros(bw, 33);
                                          
            Utils.WriteByteString(bw, "ABCDEFGHI", 9);
            bw.Write(_direction);
            Utils.WriteByteString(bw, "aaa", 100);

            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);
            bw.Write((byte)1);*/
        }

        #region Accessors
        public bool Dead
        {
            get { return false; }
        }

        public override byte MoveSpeed
        {
            get { return (byte)_moveSpeed; }
        }
        #endregion
    }

    public class MonsterTemplate
    {
        uint _id;
        uint _gameID;
        uint _hp;
        double _idleMoveChance;
        byte _idleMoveSpeed;
        byte _attackMoveSpeed;

        public Monster Instantiate(Location loc)
        {
            Monster m = new Monster(loc, this);

            return m;
        }

        public static MonsterTemplate ReadFromDB(object[] row)
        {
            // 0: template_id int(11) unsigned
            // 1: game_id int(10) unsigned
            // 2: hp  int(10) unsigned
            // 3: idle_move_chance    double
            // 4: idle_move_speed tinyint(3) unsigned
            // 5: attack_move_speed   tinyint(3) unsigned

            MonsterTemplate mt = new MonsterTemplate();

            mt._id = (uint)row[0];
            mt._gameID = (uint)row[1];
            mt._hp = (uint)row[2];
            mt._idleMoveChance = (double)row[3];
            mt._idleMoveSpeed = (byte)row[4];
            mt._attackMoveSpeed = (byte)row[5];
            return mt;
        }

        #region Accessors
        public uint ID
        {
            get { return _id; }
        }

        public uint GameID
        {
            get { return _gameID; }
        }

        public uint HP
        {
            get { return _hp; }
        }

        public byte IdleMoveSpeed
        {
            get { return _idleMoveSpeed; }
        }

        public byte AttackMoveSpeed
        {
            get { return _attackMoveSpeed; }
        }

        public double IdleMoveChance
        {
            get { return _idleMoveChance; }
        }
        #endregion
    }

    public class MonsterSpawner
    {
        uint _id;
        uint _monsterTemplateID;
        uint _locationID;
        uint _max;
        uint _rateMin;
        uint _rateMax;
        uint _intervalMin;
        uint _intervalMax;

        double _nextSpawnTime;
        List<Monster> _spawns;
        MonsterTemplate _template;
        Location _loc;
        PlayMap _map;

        private MonsterSpawner()
        {
            _spawns = new List<Monster>();
            _nextSpawnTime = 0;
        }

        public void Update(double deltaSeconds)
        {
            // Check for any dead monsters
            List<Monster> remove = new List<Monster>();
            foreach (Monster m in _spawns)
            {
                m.Update(deltaSeconds);
                if( m.Dead )
                    remove.Add(m);
            }
            foreach( Monster r in remove )
                _spawns.Remove(r);

            // Advance the spawn timer
            _nextSpawnTime -= deltaSeconds;
            if (_nextSpawnTime <= 0)
            {
                if (_spawns.Count() < _max)
                {
                    int space = (int)_max - _spawns.Count();
                    int numToSpawn = Math.Min(Program.Server.Rand.Next((int)_rateMin, (int)_rateMax), space);
                    Spawn(numToSpawn);

                    int delay = Program.Server.Rand.Next((int)_intervalMin, (int)_intervalMax);
                    _nextSpawnTime = delay;
                }
            }
        }

        void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Monster m = _template.Instantiate(_loc);
                _spawns.Add(m);
                _map.AddMonster(m);
            }
        }

        public static MonsterSpawner ReadFromDB(object[] row)
        {
            // 0: spawner_id  int(10) unsigned
            // 1: monster int(10) unsigned
            // 2: location    int(10) unsigned
            // 3: max int(10) unsigned
            // 4: rate_min    int(10) unsigned
            // 5: rate_max    int(10) unsigned
            // 6: interval_min    int(10) unsigned
            // 7: interval_max    int(10) unsigned

            MonsterSpawner ms = new MonsterSpawner();

            ms._id = (uint)row[0];
            ms._monsterTemplateID = (uint)row[1];
            ms._locationID = (uint)row[2];
            ms._max = (uint)row[3];
            ms._rateMin = (uint)row[4];
            ms._rateMax = (uint)row[5];
            ms._intervalMin = (uint)row[6];
            ms._intervalMax = (uint)row[7];

            ms._template = Program.Server.GetMonsterTemplate(ms._monsterTemplateID);
            ms._loc = Program.Server.GetLocation(ms._locationID);
            ms._map = Program.Server.GetPlayMap(ms._loc.Map);

            return ms;
        }

        #region Accessors
        public uint ID
        {
            get { return _id; }
        }
        #endregion
    }
}
