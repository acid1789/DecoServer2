using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuggleServerCore;

namespace DecoServer2
{
    public class Monster : NPC
    {
        static uint s_monsterID = 0xFFFF;
        public Monster(ushort map, uint x, uint y, uint hp, ushort gameID) : base()
        {
            s_monsterID++;
            if( s_monsterID < 0xFFFF )
                s_monsterID = 0xFFFF + 1;

            _id = s_monsterID;
            _gameID = gameID;
            _cellIndex = Utils.EncodeCellIndex(map, x, y);
            _mapID = map;
            _hp = hp;
        }

        public void SetPosition(ushort map, uint x, uint y)
        {
            _mapID = map;
            _cellIndex = Utils.EncodeCellIndex(map, x, y);
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
        #endregion
    }

    public class MonsterTemplate
    {
        uint _id;
        uint _gameID;
        uint _hp;

        public Monster Instantiate(Location loc)
        {
            int radius = (int)loc.Radius;
            int randX = Program.Server.Rand.Next(-radius, radius);
            int randY = Program.Server.Rand.Next(-radius, radius);

            uint x = (uint)((int)loc.X - randX);
            uint y = (uint)((int)loc.Y - randY);

            Monster m = new Monster(loc.Map, x, y, _hp, (ushort)_gameID);

            Console.WriteLine("Spawning monster ({0}) @ ({1},{2}) on map {3}", m.ID, x, y, m.MapID);

            return m;
        }

        public static MonsterTemplate ReadFromDB(object[] row)
        {
            // 0: template_id int(11) unsigned
            // 1: game_id int(10) unsigned
            // 2: hp  int(10) unsigned

            MonsterTemplate mt = new MonsterTemplate();

            mt._id = (uint)row[0];
            mt._gameID = (uint)row[1];
            mt._hp = (uint)row[2];

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
