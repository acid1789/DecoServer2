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
                    uint id = (uint)row[0];
                    byte type = (byte)row[2];
                    QuestRequirement qr = new QuestRequirement(id, (QuestRequirement.Type)type, (uint)row[3]);
                    q.Requirements.Add(qr);
                }

                // Fetch Steps
                sql = string.Format("SELECT * FROM quest_steps WHERE quest_id={0};", q.ID);
                rows = ExecuteQuery(sql);
                foreach( object[] row in rows )
                {
                    byte type = (byte)row[2];
                    QuestStep qs = new QuestStep((byte)row[1], (QuestStep.CompletionType)type, (uint)row[3], (uint)row[4], (uint)row[5], (uint)row[6]);
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
                        // 0: quest_id	int(10) unsigned
                        // 1: step	tinyint(3) unsigned
                        // 2: type	tinyint(10) unsigned
                        // 3: context	int(10) unsigned
                        QuestReward qr = new QuestReward((byte)row[2], (uint)row[3]);
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
                        QuestLine ql = new QuestLine((byte)row[3], (ushort)row[4], (ushort)row[5], text, (uint)row[0]);
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

        static ulong AddQuest(string name, uint giverID, ushort giverMap)
        {
            string sql = string.Format("INSERT INTO quest_info SET giver_id={0},giver_map_id={1}; SELECT LAST_INSERT_ID();", giverID, giverMap);
            List<object[]> rows = ExecuteQuery(sql);

            ulong id = (ulong)rows[0][0];

            string questName = name;
            if( name == null || name.Length < 0 )
                questName = "unnamed";
            sql = string.Format("INSERT INTO quest_names SET quest_id={0},name={1};", id, questName);
            ExecuteQuery(sql);

            return id;
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
            if (q.New)
            {
                // New quest, add it to the database now
                q.ID = (uint)AddQuest(q.Name, q.GiverID, q.GiverMapID);
            }
            else
            {
                // Update info
                ExecuteQuery(string.Format("UPDATE quest_info SET giver_id={0},giver_map_id={1} WHERE quest_id={2};", q.GiverID, q.GiverMapID, q.ID));

                // Update name
                ExecuteQuery(string.Format("UPDATE quest_names SET name=\"{0}\" WHERE quest_id={1};", q.Name, q.ID));
            }

            // Delete all the rewards for this quest
            ExecuteQuery(string.Format("DELETE FROM quest_rewards WHERE quest_id={0}", q.ID));

            // Finalize step order
            for ( int i = 0; i < q.Steps.Count; i++ )
            {
                QuestStep qs = q.Steps[i];            
                string sql;
                if( qs.New )
                    sql = string.Format("INSERT INTO quest_steps SET step={0},type={1},count={2},target_id={3},owner_id={4},quest_id={5}; SELECT LAST_INSERT_ID();", i, (byte)qs.CompType, qs.CompCount, qs.CompTargetID, qs.OwnerID, q.ID);
                else
                    sql = string.Format("UPDATE quest_steps SET step={0},type={1},count={2},target_id={3},owner_id={4} WHERE quest_step_id={5};", i, (byte)qs.CompType, qs.CompCount, qs.CompTargetID, qs.OwnerID, qs.ID);
                List<object[]> rows = ExecuteQuery(sql);
                if (rows.Count > 0)
                {
                    ulong id = (ulong)rows[0][0];
                    qs.ID = (uint)id;
                }
                qs.New = false;

                // Do Rewards
                foreach (QuestReward qr in qs.Rewards)
                {
                    sql = string.Format("INSERT INTO quest_rewards SET quest_id={0},step={1},type={2},context={3};", q.ID, i, (byte)qr.Type, qr.Context);
                    ExecuteQuery(sql);
                }

                // Do Lines
                for (int j = 0; j < qs.Lines.Count; j++)
                {
                    QuestLine ql = qs.Lines[j];
                    ql.Line = (byte)j;
                    if( ql.New )
                        sql = string.Format("INSERT INTO quest_lines SET quest_id={0},step={1},line={2},icon={3},static_text={4},text=\"{5}\"; SELECT LAST_INSERT_ID();", q.ID, i, ql.Line, ql.Icon, ql.StaticText, ql.DynamicText);
                    else
                        sql = string.Format("UPDATE quest_lines SET step={1},line={2},icon={3},static_text={4},text=\"{5}\" WHERE quest_line_id={0};", ql.ID, i, ql.Line, ql.Icon, ql.StaticText, ql.DynamicText);
                    rows = ExecuteQuery(sql);
                    if (rows.Count > 0)
                    {
                        ulong id = (ulong)rows[0][0];
                        ql.ID = (uint)id;
                    }
                    ql.New = false;
                }
            }

            // Kill any steps marked for deletion
            foreach (QuestStep qs in q.DeletedSteps)
            {
                if (!qs.New)
                {
                    string sql = string.Format("DELETE FROM quest_steps WHERE quest_step_id={0};", qs.ID);
                    ExecuteQuery(sql);
                }
            }

            // Save requirements
            foreach (QuestRequirement qr in q.Requirements)
            {
                string sql;
                if (qr.New)
                    sql = string.Format("INSERT INTO quest_requirements SET quest_id={0},type={1},param={2}; SELECT LAST_INSERT_ID();", q.ID, qr.TheType, qr.Context);
                else
                    sql = string.Format("UPDATE quest_requirements SET type={0},param={1} WHERE quest_requirement_id={2};", qr.TheType, qr.Context, qr.ID);
                List<object[]> rows = ExecuteQuery(sql);
                if (rows.Count > 0)
                {
                    ulong id = (ulong)rows[0][0];
                    qr.ID = (uint)id;
                }
                qr.New = false;
            }

            // Kill any requirements marked for deletion
            foreach (QuestRequirement qr in q.DeletedReqs)
            {
                if (!qr.New)
                {
                    string sql = string.Format("DELETE FROM quest_requirements WHERE quest_requirement_id={0};", qr.ID);
                    ExecuteQuery(sql);
                }
            }

            // Clear dirty flag
            q.Dirty = false;
        }

        public static ItemTemplate[] FetchItems()
        {
            string sql = string.Format("SELECT * FROM item_templates;");
            List<object[]> rows = ExecuteQuery(sql);

            List<ItemTemplate> templates = new List<ItemTemplate>();
            foreach (object[] row in rows)
            {
                ItemTemplate it = new ItemTemplate((uint)row[0], (ushort)row[1], (uint)row[2], (ushort)row[3], (ushort)row[4], (ushort)row[5], (ushort)row[6]);
                templates.Add(it);
            }

            return templates.ToArray();
        }

        public static void SaveItem(ItemTemplate it)
        {
            // 0: item_template_id int(10) unsigned
            // 1: model   smallint(5) unsigned
            // 2: type int(10) unsigned
            // 3: durability_min smallint(5) unsigned
            // 4: durability_max smallint(5) unsigned
            // 5: duration_min smallint(5) unsigned
            // 6: duration_max smallint(5) unsigned

            string sql;
            if( it.New )
                sql = string.Format("INSERT INTO item_templates SET model={0},type={1},durability_min={2},durability_max={3},duration_min={4},duration_max={5}; SELECT LAST_INSERT_ID();", it.Model, (int)it.Type, it.DurabilityMin, it.DurabilityMax, it.DurationMin, it.DurationMax);
            else
                sql = string.Format("UPDATE item_templates SET model={0},type={1},durability_min={2},durability_max={3},duration_min={4},duration_max={5} WHERE item_template_id={6};", it.Model, (int)it.Type, it.DurabilityMin, it.DurabilityMax, it.DurationMin, it.DurationMax, it.ID);
            List<object[]> rows = ExecuteQuery(sql);
            if (rows.Count > 0)
            {
                ulong id = (ulong)rows[0][0];
                it.ID = (uint)id;
            }

            it.New = false;
            it.Dirty = false;
        }

        public static void DeleteItem(ItemTemplate it)
        {
            if (!it.New)
            {
                string sql = string.Format("DELETE FROM item_templates WHERE item_template_id={0};", it.ID);
                ExecuteQuery(sql);
            }
        }

        public static Location[] FetchLocations(ushort mapID)
        {
            // 0: location_id int(10) unsigned
            // 1: name    varchar(30)
            // 2: x   int(10) unsigned
            // 3: y   int(10) unsigned
            // 4: radius  int(10) unsigned
            // 5: map	smallint(5) unsigned

            List<Location> locs = new List<Location>();

            string sql = string.Format("SELECT * FROM locations;");
            if( mapID != 0 )
                sql = string.Format("SELECT * FROM locations WHERE map={0};", mapID);
            List<object[]> rows = ExecuteQuery(sql);

            foreach (object[] row in rows)
            {
                locs.Add(new Location((uint)row[0], (string)row[1], (uint)row[2], (uint)row[3], (uint)row[4], (ushort)row[5]));
            }
            
            return locs.ToArray();
        }

        public static void UpdateLocation(Location loc)
        {
            string sql = string.Format("UPDATE locations SET name=\"{0}\",x={1},y={2},radius={3},map={4} WHERE location_id={5};", loc.Name, loc.X, loc.Y, loc.Radius, loc.Map, loc.ID);
            ExecuteQuery(sql);
        }

        public static Location CreateLocation(uint x, uint y, ushort map)
        {
            Location loc = new Location(0, "Unnamed", x, y, 1, map);
            string sql = string.Format("INSERT INTO locations SET name=\"{0}\",x={1},y={2},radius={3},map={4}; SELECT LAST_INSERT_ID();", loc.Name, loc.X, loc.Y, loc.Radius, loc.Map, loc.ID);
            List<object[]> rows = ExecuteQuery(sql);

            ulong id = (ulong)rows[0][0];
            loc.ID = (uint)id;
            return loc;
        }

        public static void DeleteLocation(Location loc)
        {
            string sql = string.Format("DELETE FROM locations WHERE location_id={0};", loc.ID);
            ExecuteQuery(sql);
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

    public class ItemTemplate
    {
        public enum ItemType
        {
            Clothing,
            General,
            Item,
            Quest,
            Riding
        }


        public uint ID;
        public ushort Model;
        public ItemType Type;
        public ushort DurabilityMin;
        public ushort DurabilityMax;
        public ushort DurationMin;
        public ushort DurationMax;
        public bool New;
        public bool Dirty;

        public ItemTemplate(uint id, ushort model, uint type, ushort durabilityMin, ushort durabilityMax, ushort durationMin, ushort durationMax)
        {
            ID = id;
            Model = model;
            Type = (ItemType)type;
            DurabilityMin = durabilityMin;
            DurabilityMax = durabilityMax;
            DurationMin = durationMin;
            DurationMax = durationMax;
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
        public bool New;

        public List<QuestRequirement> DeletedReqs;
        public List<QuestStep> DeletedSteps;

        public Quest(uint id, uint giver, ushort giverMap)
        {
            ID = id;
            GiverID = giver;
            GiverMapID = giverMap;
            Name = "unnamed";
            Requirements = new List<QuestRequirement>();
            Steps = new List<QuestStep>();

            DeletedReqs = new List<QuestRequirement>();
            DeletedSteps = new List<QuestStep>();
        }

        public void DeleteStep(QuestStep qs)
        {
            Steps.Remove(qs);
            if( !qs.New )
                DeletedSteps.Add(qs);
        }

        public void DeleteRequirement(QuestRequirement qr)
        {
            Requirements.Remove(qr);
            if( !qr.New )
                DeletedReqs.Add(qr);
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

        public uint ID;
        public Type TheType;
        public uint Context;
        public bool New;

        public QuestRequirement(uint id, Type type, uint context)
        {
            ID = id;
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
            ReachLevel,
            WearItem,
        }

        public byte Step;
        public CompletionType CompType;
        public uint CompCount;
        public uint CompTargetID;
        public uint OwnerID;
        public uint ID;
        public List<QuestReward> Rewards;
        public List<QuestLine> Lines;
        public bool New;

        List<QuestReward> DeletedRewards;
        List<QuestLine> DeletedLines;

        public QuestStep(byte step, CompletionType type, uint compCount, uint compTarget, uint owner, uint id)
        {
            DeletedLines = new List<QuestLine>();
            DeletedRewards = new List<QuestReward>();

            Step = step;
            CompType = type;
            CompCount = compCount;
            CompTargetID = compTarget;
            OwnerID = owner;
            ID = id;

            Rewards = new List<QuestReward>();
            Lines = new List<QuestLine>();
        }

        public void DeleteReward(QuestReward qr)
        {
            Rewards.Remove(qr);
            if( !qr.New )
                DeletedRewards.Add(qr);
        }

        public void DeleteLine(QuestLine ql)
        {
            Lines.Remove(ql);
            if (!ql.New)
                DeletedLines.Add(ql);
        }

        public void OrderLines()
        {
            List<QuestLine> ordered = Lines.OrderBy(o => o.Line).ToList();
            Lines = ordered;
        }
    }

    public class QuestReward
    {
        public enum RewardType
        {
            Gold,
            Exp,
            Fame,
            Item,
            Teleport,
            Skill
        }
        public RewardType Type;
        public uint Context;
        public bool New;

        public QuestReward(byte type, uint context)
        {
            Type = (RewardType)type;
            Context = context;
        }
    }

    public class QuestLine
    {
        public byte Line;
        public ushort Icon;
        public ushort StaticText;
        public string DynamicText;
        public bool New;
        public uint ID;

        public QuestLine(byte line, ushort icon, ushort staticText, string dynamicText, uint id)
        {
            Line = line;
            Icon = icon;
            StaticText = staticText;
            DynamicText = dynamicText;
            ID = id;
        }
    }

    public class Location
    {
        public uint ID;
        public string Name;
        public uint X;
        public uint Y;
        public uint Radius;
        public ushort Map;
        public bool Dirty;

        public Location(uint id, string name, uint x, uint y, uint radius, ushort map)
        {
            ID = id;
            Name = name;
            X = x;
            Y = y;
            Radius = radius;
            Map = map;
        }

        public override string ToString()
        {
            return ID.ToString() + ": " + Name + "(" + Map.ToString() + ")";
        }
    }
}
