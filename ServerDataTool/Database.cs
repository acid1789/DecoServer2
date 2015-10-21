using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ServerDataTool
{
    static class Database
    {
        static MySqlConnection _sql;
        static string _dbConnectString;

        public static void Init(string dbString)
        {
            _dbConnectString = dbString;
            ValidateConnection();
        }

        static void ValidateConnection()
        {
            if (_sql == null)
            {
                try
                {
                    _sql = new MySqlConnection();
                    _sql.ConnectionString = _dbConnectString;
                    _sql.Open();

                    Thread.Sleep(10);

                    while (_sql.State == ConnectionState.Connecting)
                        Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    _sql.Close();
                    _sql = null;
                }
            }
            else
            {
                if (_sql.State == ConnectionState.Closed || _sql.State == ConnectionState.Broken)
                {
                    _sql.Close();
                    _sql = null;
                }
            }
        }

        static List<object[]> ExecuteQuery(string query)
        {
            List<object[]> rows = new List<object[]>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, _sql);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader[i];
                    }
                    rows.Add(row);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return rows;            
        }

        static public DBAccountRow[] FetchAccounts()
        {
            string sql = string.Format("SELECT * FROM accounts;");
            List<object[]> rows = ExecuteQuery(sql);

            List<DBAccountRow> accounts = new List<DBAccountRow>();
            foreach (object[] row in rows)
            {
                int id = (int)row[0];
                string name = (string)row[1];
                string pw = (string)row[2];
                accounts.Add(new DBAccountRow(id, name, pw));
            }

            return accounts.ToArray();
        }

        static public DBAccountRow AddAccount(string accountName, string password)
        {
            string sql = string.Format("INSERT INTO accounts SET user_name=\"{0}\",password=\"{1}\"; SELECT LAST_INSERT_ID();", accountName, password);
            List<object[]> rows = ExecuteQuery(sql);

            DBAccountRow account = new DBAccountRow(-1, accountName, password);
            if (rows.Count > 0)
            {
                ulong id = (ulong)rows[0][0];
                account.ID = (int)id;
            }

            return account;
        }

        static public ushort[] FetchMaps()
        {
            List<object[]> rows = ExecuteQuery("SELECT * FROM play_maps;");

            List<ushort> maps = new List<ushort>();
            foreach (object[] row in rows)
            {
                maps.Add((ushort)row[0]);
            }
            return maps.ToArray();
        }

        static public void AddMap(ushort map)
        {
            string sql = string.Format("INSERT INTO play_maps SET map_id={0};", map);
            ExecuteQuery(sql);
        }

        static public NPC[] FetchNPCs(ushort mapID)
        {
            List<NPC> npcs = new List<NPC>();

            string sql;
            if( mapID == 0 )
                sql = string.Format("SELECT * FROM static_npcs;");
            else 
                sql = string.Format("SELECT * FROM static_npcs WHERE map_id={0};", mapID);
            List<object[]> rows = ExecuteQuery(sql);
            foreach (object[] row in rows)
            {
                npcs.Add(new NPC((uint)row[0], (ushort)row[1], (uint)row[2], (uint)row[3], (uint)row[4], (uint)row[5], (ushort)row[6]));
            }
            return npcs.ToArray();
        }

        static public void UpdateNPC(NPC npc)
        {
            string sql = string.Format("UPDATE static_npcs SET game_id={0},location_x={1},location_y={2},hp={3},direction={4} WHERE static_npc_id={5};", npc.GameID, npc.X, npc.Y, npc.HP, npc.Direction, npc.ID);
            ExecuteQuery(sql);
        }

        static public NPC CreateNPC(int x, int y, ushort mapId)
        {
            string sql = string.Format("INSERT INTO static_npcs SET game_id={0},location_x={1},location_y={2},map_id={3}; SELECT LAST_INSERT_ID();", 5000, x, y, mapId);
            List<object[]> rows = ExecuteQuery(sql);
            
            ulong id = (ulong)rows[0][0];
            NPC npc = new NPC((uint)id, 5000, (uint)x, (uint)y, 10000, 0, mapId);
            return npc;
        }

        static public void DeleteNPC(NPC npc)
        {
            string sql = string.Format("DELETE FROM static_npcs WHERE static_npc_id={0};", npc.ID);
            ExecuteQuery(sql);
        }

        static public Quest[] FetchQuests()
        {
            List<Quest> quests = new List<Quest>();

            // Fetch Quests
            List<object[]> rows = ExecuteQuery("SELECT * FROM quest_info;");
            foreach (object[] row in rows)
            {
                Quest q = new Quest((uint)row[0], (uint)row[1], (ushort)row[2]);
                quests.Add(q);
            }

            // Go get subquest data
            foreach (Quest q in quests)
            {
                // Fetch Name
                string sql = string.Format("SELECT * FROM quest_names WHERE quest_id={0};", q.ID);
                rows = ExecuteQuery(sql);
                if( rows.Count > 0 )
                    q.Name = (string)rows[0][1];

                // Fetch Requirements
                sql = string.Format("SELECT * FROM quest_requirements WHERE quest_id={0};", q.ID);
                rows = ExecuteQuery(sql);
                foreach (object[] row in rows)
                {
                    byte type = (byte)row[1];
                    QuestRequirement qr = new QuestRequirement((QuestRequirement.Type)type, (uint)row[2]);
                    q.Requirements.Add(qr);
                }

                // Fetch Steps
                sql = string.Format("SELECT * FROM quest_steps WHERE quest_id={0};", q.ID);
                rows = ExecuteQuery(sql);
                foreach( object[] row in rows )
                {
                    byte type = (byte)row[2];
                    QuestStep qs = new QuestStep((byte)row[1], (QuestStep.CompletionType)type, (uint)row[3], (uint)row[4], (uint)row[5]);
                    q.Steps.Add(qs);
                }

                // Process Steps
                foreach (QuestStep qs in q.Steps)
                {
                    // Fetch Rewards
                    sql = string.Format("SELECT * FROM quest_rewards WHERE quest_id={0} AND step={1};", q.ID, qs.Step);
                    rows = ExecuteQuery(sql);
                    foreach (object[] row in rows)
                    {
                        QuestReward qr = new QuestReward((uint)row[2], (uint)row[3], (uint)row[6], (uint)row[4]);
                        qs.Rewards.Add(qr);
                    }

                    // Fetch Lines
                    sql = string.Format("SELECT * FROM quest_lines WHERE quest_id={0} AND step={1};", q.ID, qs.Step);
                    rows = ExecuteQuery(sql);
                    foreach (object[] row in rows)
                    {
                        string text = null;
                        if (row[6].GetType() != typeof(DBNull))
                            text = (string)row[6];
                        QuestLine ql = new QuestLine((byte)row[3], (ushort)row[4], (ushort)row[5], text);
                        qs.Lines.Add(ql);
                    }

                    // Order Lines
                    qs.OrderLines();                    
                }

                // Order Steps
                q.OrderSteps();
            }
            return quests.ToArray();
        }

        public static Quest AddQuest(string name, uint giverID, ushort giverMap)
        {
            string sql = string.Format("INSERT INTO quest_info SET giver_id={0},giver_map_id={1}; SELECT LAST_INSERT_ID();", giverID, giverMap);
            List<object[]> rows = ExecuteQuery(sql);

            ulong id = (ulong)rows[0][0];

            string questName = name;
            if( name == null || name.Length < 0 )
                questName = "unnamed";
            sql = string.Format("INSERT INTO quest_names SET quest_id={0},name={1};", id, questName);
            ExecuteQuery(sql);

            Quest q = new Quest((uint)id, giverID, giverMap);
            return q;
        }

        public static void DeleteQuest(Quest q)
        {
            string sql = "";
            sql += string.Format("DELETE FROM quest_requirements WHERE quest_id={0};", q.ID);
            sql += string.Format("DELETE FROM quest_steps WHERE quest_id={0};", q.ID);
            sql += string.Format("DELETE FROM quest_rewards WHERE quest_id={0};", q.ID);
            sql += string.Format("DELETE FROM quest_lines WHERE quest_id={0};", q.ID);
            sql += string.Format("DELETE FROM quest_names WHERE quest_id={0};", q.ID);
            sql += string.Format("DELETE FROM quest_info WHERE quest_id={0};", q.ID);
            ExecuteQuery(sql); 
        }

        public static void SaveQuest(Quest q)
        {
        }
    }

    public class DBAccountRow
    {
        public int ID;
        public string UserName;
        public string Password;

        public DBAccountRow(int id, string name, string pass)
        {
            ID = id;
            UserName = name;
            Password = pass;
        }
    }

    public class NPC
    {
        public uint ID;
        public ushort GameID;
        public uint X;
        public uint Y;
        public uint HP;
        public uint Direction;
        public ushort MapID;
        public bool Dirty;

        public NPC(uint id, ushort gameID, uint x, uint y, uint hp, uint direction, ushort mapID)
        {
            ID = id;
            GameID = gameID;
            X = x;
            Y = y;
            HP = hp;
            Direction = direction;
            MapID = mapID;
            Dirty = false;
        }

        public override string ToString()
        {
            return ID.ToString() + ": " + Program.s_npcNameIDs[GameID].ToString();
        }
    }

    public class Quest
    {
        public uint ID;
        public string Name;
        public uint GiverID;
        public ushort GiverMapID;
        public List<QuestRequirement> Requirements;
        public List<QuestStep> Steps;
        public bool Dirty;

        public Quest(uint id, uint giver, ushort giverMap)
        {
            ID = id;
            GiverID = giver;
            GiverMapID = giverMap;
            Name = "unnamed";
            Requirements = new List<QuestRequirement>();
            Steps = new List<QuestStep>();
        }

        public void OrderSteps()
        {
            List<QuestStep> ordered = Steps.OrderBy(o => o.Step).ToList();
            Steps = ordered;
        }
    }

    public class QuestRequirement
    {
        public enum Type
        {
            Level,
            Race,
            Gender,
            Job,
            Fame,
            Item
        }

        public Type TheType;
        public uint Context;
        public bool New;

        public QuestRequirement(Type type, uint context)
        {
            TheType = type;
            Context = context;
        }
    }

    public class QuestStep
    {
        public enum CompletionType
        {
            KillMonster,
            CollectItem,
            GoToLocation,
            TalkToNPC,
            ReachLevel
        }

        public byte Step;
        public CompletionType CompType;
        public uint CompCount;
        public uint CompTargetID;
        public uint OwnerID;
        public List<QuestReward> Rewards;
        public List<QuestLine> Lines;

        public QuestStep(byte step, CompletionType type, uint compCount, uint compTarget, uint owner)
        {
            Step = step;
            CompType = type;
            CompCount = compCount;
            CompTargetID = compTarget;
            OwnerID = owner;

            Rewards = new List<QuestReward>();
            Lines = new List<QuestLine>();
        }

        public void OrderLines()
        {
            List<QuestLine> ordered = Lines.OrderBy(o => o.Line).ToList();
            Lines = ordered;
        }
    }

    public class QuestReward
    {
        public uint Gold;
        public uint Exp;
        public uint Fame;
        public uint Item;

        public QuestReward(uint gold, uint exp, uint fame, uint item)
        {
            Gold = gold;
            Exp = exp;
            Fame = fame;
            Item = item;
        }
    }

    public class QuestLine
    {
        public byte Line;
        public ushort Icon;
        public ushort StaticText;
        public string DynaimcText;

        public QuestLine(byte line, ushort icon, ushort staticText, string dynamicText)
        {
            Line = line;
            Icon = icon;
            StaticText = staticText;
            DynaimcText = dynamicText;
        }
    }
}
