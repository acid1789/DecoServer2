using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using JuggleServerCore;

namespace DecoServer2
{
    public class Monster : NPC
    {
        enum AIState
        {
            Idle,
            Attack,
            Dead,
        };
        
        MonsterTemplate _template;
        Location _loc;

        AIState _aiState;
        double _aiTimer;

        Vector _moveTarget;
        uint _moveTargetCell;
        float _moveSpeed;
        double _moveTimer;

        uint _curhp;
        Dictionary<int, uint> _enemies;
        Mutex _enemyLock;


        static uint s_monsterID = 0xFFFF;
        public Monster(Location loc, MonsterTemplate template) : base()
        {
            s_monsterID++;
            if( s_monsterID < 0xFFFF )
                s_monsterID = 0xFFFF + 1;

            _enemies = new Dictionary<int, uint>();
            _enemyLock = new Mutex();

            _loc = loc;
            _template = template;

            _id = s_monsterID;
            _gameID = (ushort)_template.GameID;
            _cellIndex = _moveTargetCell = loc.RandomCell;
            _mapID = loc.Map;
            _hp = _curhp = _template.HP;
            _aiState = AIState.Idle;
            _aiTimer = 0;

            _moveTimer = 0;
        }

        public void Update(double deltaSeconds)
        {
            Connection enemy = FindEnemy();
            if (enemy != null && _aiState != AIState.Attack)
            {
                // There is a valid enemy, go into attack mode!
                _aiState = AIState.Attack;
                _aiTimer = 0;
            }
            else if (enemy == null && _aiState != AIState.Idle)
            {
                _aiState = AIState.Idle;
                _aiTimer = 0;
            }

            if( _curhp <= 0 )
                _aiState = AIState.Dead;


            if (_aiTimer > 0)
                _aiTimer -= deltaSeconds;

            UpdateMove(deltaSeconds);
            UpdateAI(deltaSeconds, enemy);            
        }

        void UpdateMove(double deltaSeconds)
        {            
            if (_cellIndex != _moveTargetCell)
            {
                _moveTimer -= deltaSeconds;
                if (_moveTimer <= 0)
                {
                    // Move along the vector
                    Vector pos = Utils.DecodeCellIndex(_loc.Map, _cellIndex);
                    Vector delta = _moveTarget - pos;
                    double dist = delta.Length;
                    if (dist > _moveSpeed)
                        dist = _moveSpeed;

                    delta.Normalize();
                    Vector target = pos + (delta * dist);

                    _cellIndex = Utils.EncodeCellIndex(_loc.Map, (uint)target.X, (uint)target.Y);
                    Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.UpdateNPCPosition, null, this));
                    _moveTimer = 1;
                }
            }
        }

        void UpdateAI(double deltaSeconds, Connection enemy)
        {
            switch (_aiState)
            {
                case AIState.Idle:
                    if (_aiTimer <= 0)
                    {
                        if (Program.Server.Rand.NextDouble() < _template.IdleMoveChance)
                        {
                            // Wander to a new spot
                            _moveTargetCell = _loc.RandomCell;
                            _moveTarget = Utils.DecodeCellIndex(_loc.Map, _moveTargetCell);
                            _moveSpeed = _template.IdleMoveSpeed;
                        }
                        else
                            _aiTimer = 1;
                    }
                    break;
                case AIState.Attack:
                    DoAttack(enemy);                    
                    break;
                case AIState.Dead:
                    break;
            }
        }

        void DoAttack(Connection enemy)
        {
            Vector myPos = Utils.DecodeCellIndex(_loc.Map, _cellIndex);
            Vector ePos = Utils.DecodeCellIndex(enemy.Character.MapID, enemy.Character.CellIndex);

            Vector fromEnemy = myPos - ePos;
            double dist = fromEnemy.Length;
            if (dist > _template.IdleMoveSpeed)
            {
                fromEnemy.Normalize();
                fromEnemy *= 2;
                _moveTarget = ePos + fromEnemy;
                _moveTargetCell = Utils.EncodeCellIndex(_mapID, _moveTarget);
                _moveSpeed = _template.IdleMoveSpeed;
                _aiTimer = 1;
                UpdateMove(0);
            }

            if (_aiTimer <= 0 && dist < _template.IdleMoveSpeed)
            {
                // Attack the player
                bool critical = ( Program.Server.Rand.NextDouble() < _template.CriticalChance );
                int damage = Program.Server.Rand.Next(_template.AttackMin, _template.AttackMax);
                if( critical )
                    damage *= 2;
                
                ushort attackType = (ushort)(critical ? 3 : 1);
                Program.Server.TaskProcessor.AddTask(new Task(Task.TaskType.MonsterAttackPlayer, enemy, new object[] { this, damage, attackType }));

                _aiTimer = _template.AttackDelay;
            }
        }

        Connection FindEnemy()
        {
            Connection enemy = null;
            if (_enemies.Count > 0)
            {
                // Grab all the enemies
                _enemyLock.WaitOne();
                KeyValuePair<int, uint>[] enemies = _enemies.ToArray();
                _enemyLock.ReleaseMutex();
                List<int> toRemove = new List<int>();

                // Pick the one that has done the most damage
                PlayMap myMap = Program.Server.GetPlayMap(_mapID);
                uint bestThreatVal = 0;
                Connection bestThreat = null;
                foreach (KeyValuePair<int, uint> kvp in enemies)
                {
                    Connection e = myMap.GetPlayer(kvp.Key);
                    if (e.Character.CurHP <= 0 || kvp.Value == 0)
                        toRemove.Add(kvp.Key);
                    else if (kvp.Value > bestThreatVal)
                    {
                        bestThreatVal = kvp.Value;
                        bestThreat = e;
                    }
                }

                enemy = bestThreat;

                // Remove any dead guys
                if (toRemove.Count > 0)
                {
                    _enemyLock.WaitOne();
                    foreach (int remove in toRemove)
                        _enemies.Remove(remove);
                    _enemyLock.ReleaseMutex();
                }
            }
            return enemy;
        }

        public void TakeDamage(uint damage, int fromId)
        {
            _enemyLock.WaitOne();
            uint prevDmg = _enemies.ContainsKey(fromId) ? _enemies[fromId] : 0;
            _enemies[fromId] = prevDmg + damage + 1;
            _enemyLock.ReleaseMutex();

            if( damage > _curhp )
                _curhp = 0;
            else
                _curhp -= damage;
           
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
            get { return _curhp <= 0; }
        }

        public override byte MoveSpeed
        {
            get { return (byte)_moveSpeed; }
        }

        public uint CurHP
        {
            get { return _curhp; }
            set { _curhp = value; }
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
        double _criticalChance;
        double _attackDelay;
        ushort _attackMin;
        ushort _attackMax;

        public Monster Instantiate(Location loc)
        {
            Monster m = new Monster(loc, this);

            return m;
        }

        public static MonsterTemplate ReadFromDB(object[] row)
        {
            // 0: template_id	int(11) unsigned
            // 1: game_id int(10) unsigned
            // 2: hp  int(10) unsigned
            // 3: idle_move_chance    double
            // 4: idle_move_speed tinyint(3) unsigned
            // 5: critical_chance double
            // 6: attack_delay    double
            // 7: attack_min  smallint(5) unsigned
            // 8: attack_max  smallint(5) unsigned
            
            MonsterTemplate mt = new MonsterTemplate();

            mt._id = (uint)row[0];
            mt._gameID = (uint)row[1];
            mt._hp = (uint)row[2];
            mt._idleMoveChance = (double)row[3];
            mt._idleMoveSpeed = (byte)row[4];
            mt._criticalChance = (double)row[5];
            mt._attackDelay = (double)row[6];
            mt._attackMin = (ushort)row[7];
            mt._attackMax = (ushort)row[8];
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

        public double IdleMoveChance
        {
            get { return _idleMoveChance; }
        }

        public double CriticalChance
        {
            get { return _criticalChance; }
        }

        public int AttackMin
        {
            get { return _attackMin; }
        }

        public int AttackMax
        {
            get { return _attackMax; }
        }

        public double AttackDelay
        {
            get { return _attackDelay; }
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
            if (remove.Count > 0)
            {
                foreach (Monster r in remove)
                    _spawns.Remove(r);
                _nextSpawnTime = _intervalMin;
            }

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
            ms._max = 2;
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
