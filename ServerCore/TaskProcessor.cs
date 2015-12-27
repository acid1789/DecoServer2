using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DecoServer2.CharacterThings;
using DecoServer2;
using DecoServer2.Quests;

namespace JuggleServerCore
{    
    public class Task
    {
        public enum TaskType
        {
            LoadPlayMaps_Fetch,
            LoadPlayMaps_Process,
            LoadNPCs_Fetch,
            LoadNPCs_Process,
            LoadItems_Process,
            LoadLootTables_Process,
            LoadLocations_Process,
            LoadMonsterTemplates_Process,
            LoadMonsterSpawners_Process,
            LoadQuestLines_Process,
            LoadQuestSteps_Process,
            LoadQuestRewards_Process,
            LoadQuestRequirements_Process,
            LoadQuestInfo_Process,            

            LoginRequest_Fetch,
            LoginRequest_Process,
            CharacterList_Fetch,
            CharacterList_Process,
            CreateCharacter,
            CreateCharacter_Finish,
            DeleteCharacter,
            SelectCharacter,
            SelectedCharacter_Fetch,
            CharacterDataHV_Process,
            CharacterDataLV_Process,
            CharacterDataItems_Process,
            CharacterFrontierData_Process,
            CharacterSkills_Process,
            CharacterActiveQuests_Process,
            CharacterCompletedQuests_Process,
            CharacterToolbar_Process,
            CharacterActiveQuest_Save,
            PlayerEnterMap,
            PlayerMove,
            PlayerUpdatePosition,
            NPCDialogNextButton,
            GiveGoldExpFame,
            GiveItem,
            GiveItem_Finish,
            RemoveCharacter,
            GMCommand_Process,
            MoveItem,
            EquipItem,
            UnEquipItem,
            Teleport,
            UpdateNPCPosition,
            DoAttack,
            MonsterAttackPlayer,
            UseItem,
            ToolbarItemSet,
            ToolbarItemClear,
        }

        public TaskType Type;
        public DBQuery Query;

        protected Connection _client;
        protected object _args;

        public Task(int typeVal)
        {
            Type = (TaskType)typeVal;
        }

        public Task(TaskType type, Connection client = null, object args = null)
        {
            Type = type;
            _client = client;
            _args = args;
            Query = null;
        }
        
        public object Args
        {
            get { return _args; }
            set { _args = value; }
        }

        public Connection Client
        {
            get { return _client; }
        }
    }

    public class TaskProcessor
    {
        List<Task> _tasks;
        Mutex _tasksLock;
        bool _processing;

        long _lastTicks;
        long _ticksModified;

        DatabaseThread _db;

        protected delegate void TaskHandler(Task task);
        protected Dictionary<Task.TaskType, TaskHandler> _taskHandlers;

        protected Dictionary<long, Task> _pendingQueries;
        protected Mutex _pqLock;

        ServerBase _server;

        public TaskProcessor(ServerBase server)
        {
            _server = server;

            _tasks = new List<Task>();
            _tasksLock = new Mutex();

            _taskHandlers = new Dictionary<Task.TaskType, TaskHandler>();
            _taskHandlers[Task.TaskType.LoadPlayMaps_Fetch] = LoadPlayMaps_Fetch_Handler;
            _taskHandlers[Task.TaskType.LoadPlayMaps_Process] = LoadPlayMaps_Process_Handler;
            _taskHandlers[Task.TaskType.LoadNPCs_Fetch] = LoadNPCs_Fetch_Handler;
            _taskHandlers[Task.TaskType.LoadNPCs_Process] = LoadNPCs_Process_Handler;
            _taskHandlers[Task.TaskType.LoadItems_Process] = LoadItems_Process_Handler;
            _taskHandlers[Task.TaskType.LoadLootTables_Process] = LoadLootTables_Process_Handler;
            _taskHandlers[Task.TaskType.LoadLocations_Process] = LoadLocations_Process_Handler;
            _taskHandlers[Task.TaskType.LoadMonsterTemplates_Process] = LoadMonsterTemplates_Process_Handler;
            _taskHandlers[Task.TaskType.LoadMonsterSpawners_Process] = LoadMonsterSpawners_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestLines_Process] = LoadQuestLines_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestSteps_Process] = LoadQuestSteps_Process_Handler;
            _taskHandlers[Task.TaskType.LoginRequest_Fetch] = LoginRequest_Fetch_Handler;
            _taskHandlers[Task.TaskType.LoginRequest_Process] = LoginRequest_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestRewards_Process] = LoadQuestRewards_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestRequirements_Process] = LoadQuestRequirements_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestInfo_Process] = LoadQuestInfo_Process_Handler;

            _taskHandlers[Task.TaskType.CharacterList_Fetch] = CharacterList_Fetch_Handler;
            _taskHandlers[Task.TaskType.CharacterList_Process] = CharacterList_Process_Handler;
            _taskHandlers[Task.TaskType.CreateCharacter] = CreateCharacter_Handler;
            _taskHandlers[Task.TaskType.CreateCharacter_Finish] = CreateCharacter_Finish_Handler;
            _taskHandlers[Task.TaskType.DeleteCharacter] = DeleteCharacter_Handler;
            _taskHandlers[Task.TaskType.SelectCharacter] = SelectCharacter_Handler;
            _taskHandlers[Task.TaskType.SelectedCharacter_Fetch] = SelectedCharacter_Fetch_Handler;
            _taskHandlers[Task.TaskType.CharacterDataHV_Process] = CharacterDataHV_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterDataLV_Process] = CharacterDataLV_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterDataItems_Process] = CharacterDataItems_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterFrontierData_Process] = CharacterFrontierData_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterSkills_Process] = CharacterSkills_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterActiveQuests_Process] = CharacterActiveQuests_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterCompletedQuests_Process] = CharacterCompletedQuests_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterToolbar_Process] = CharacterToolbar_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterActiveQuest_Save] = CharacterActiveQuest_Save_Handler;
            _taskHandlers[Task.TaskType.PlayerEnterMap] = PlayerEnterMap_Handler;
            _taskHandlers[Task.TaskType.PlayerMove] = PlayerMove_Handler;
            _taskHandlers[Task.TaskType.PlayerUpdatePosition] = PlayerUpdatePosition_Handler;
            _taskHandlers[Task.TaskType.NPCDialogNextButton] = NPCDialogNextButton_Handler;
            _taskHandlers[Task.TaskType.GiveGoldExpFame] = GiveGoldExpFame_Handler;
            _taskHandlers[Task.TaskType.GiveItem] = GiveItem_Handler;
            _taskHandlers[Task.TaskType.GiveItem_Finish] = GiveItem_Finish_Handler;
            _taskHandlers[Task.TaskType.RemoveCharacter] = RemoveCharacter_Handler;
            _taskHandlers[Task.TaskType.GMCommand_Process] = GMCommand_Process_Handler;
            _taskHandlers[Task.TaskType.MoveItem] = MoveItem_Handler;
            _taskHandlers[Task.TaskType.EquipItem] = EquipItem_Handler;
            _taskHandlers[Task.TaskType.UnEquipItem] = UnEquipItem_Handler;
            _taskHandlers[Task.TaskType.Teleport] = Teleport_Handler;
            _taskHandlers[Task.TaskType.UpdateNPCPosition] = UpdateNPCPosition_Handler;
            _taskHandlers[Task.TaskType.DoAttack] = DoAttack_Handler;
            _taskHandlers[Task.TaskType.MonsterAttackPlayer] = MonsterAttackPlayer_Handler;
            _taskHandlers[Task.TaskType.UseItem] = UseItem_Handler;
            _taskHandlers[Task.TaskType.ToolbarItemSet] = ToolbarItemSet_Handler;
            _taskHandlers[Task.TaskType.ToolbarItemClear] = ToolbarItemClear_Handler;

            _pendingQueries = new Dictionary<long, Task>();
            _pqLock = new Mutex();
        }

        public void Database_OnQueryComplete(object sender, EventArgs e)
        {
            DBQuery q = (DBQuery)sender;

            _pqLock.WaitOne();
            LogInterface.Log("Finishing Query with key: " + q.Key, LogInterface.LogMessageType.Debug);
            Task task = _pendingQueries[q.Key];
            _pendingQueries.Remove(q.Key);
            _pqLock.ReleaseMutex();

            // reschedule the task to deal with the new data\
            if( task.Type >= 0 )
                AddTask(task);
        }

        long UniqueKey()
        {
            long ticks = DateTime.Now.Ticks;
            long key = ticks;
            if (_lastTicks == ticks)
                key += ++_ticksModified;
            else
                _ticksModified = 0;
            _lastTicks = ticks;
            return key;
        }

        public DBQuery AddDBQuery(string sql, Task task, bool read = true)
        {
            if( task == null )
                task = new Task(-1);

            long key = UniqueKey();
            DBQuery q = new DBQuery(sql, read, key);
            
            task.Query = q;

            _pqLock.WaitOne();
            LogInterface.Log("Adding Query with key: " + key, LogInterface.LogMessageType.Debug, true);
            _pendingQueries[key] = task;
            _pqLock.ReleaseMutex();

            _db.AddQuery(q);
            return q;
        }

        public void AddTask(Task t)
        {
            _tasksLock.WaitOne();
            _tasks.Add(t);
            _tasksLock.ReleaseMutex();
        }

        public void Process()
        {
            _processing = true;
            while (_processing)
            {
                try
                {
                    if (_tasks.Count > 0)
                    {
                        // Grab the first task
                        _tasksLock.WaitOne();
                        Task t = _tasks[0];
                        _tasks.RemoveAt(0);
                        _tasksLock.ReleaseMutex();

                        // Execute the task
                        ProcessTask(t);
                    }
                }
                catch (Exception ex)
                {
                    LogThread.Log(ex.ToString(), LogThread.LogMessageType.Error, false);
                }

                Thread.Sleep(10);
            }
        }

        void ProcessTask(Task t)
        {
            // Call the task handler, this will throw an exception if the handler isnt registered.
            LogThread.Log(string.Format("ProcessTask({0}) -> {1}", t.Type, _taskHandlers[t.Type].Method.Name), LogThread.LogMessageType.Debug);
            _taskHandlers[t.Type](t);
        }

        #region Accessors
        public DatabaseThread Database
        {
            get { return _db; }
            set { _db = value; }       
        }
        #endregion

        #region Data Loading Handlers
        void LoadPlayMaps_Fetch_Handler(Task t)
        {
            t.Type = Task.TaskType.LoadPlayMaps_Process;
            AddDBQuery("SELECT * FROM play_maps;", t);
        }

        void LoadPlayMaps_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                ushort mapID = (ushort)row[0];
                _server.AddPlayMap(mapID);
            }

            t.Type = Task.TaskType.LoadNPCs_Fetch;
            AddTask(t);
        }

        void LoadNPCs_Fetch_Handler(Task t)
        {
            t.Type = Task.TaskType.LoadNPCs_Process;
            AddDBQuery("SELECT * FROM static_npcs;", t);
        }

        void LoadNPCs_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                NPC npc = NPC.ReadFromDB(row);
                _server.AddNPC(npc);
            }

            t.Type = Task.TaskType.LoadItems_Process;
            AddDBQuery("SELECT * FROM item_templates;", t);
        }

        void LoadItems_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                ItemTemplate template = ItemTemplate.ReadFromDB(row);
                _server.AddItemTemplate(template);
            }

            t.Type = Task.TaskType.LoadLootTables_Process;
            AddDBQuery("SELECT * FROM loot_tables;", t);
        }

        void LoadLootTables_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                // 0: loot_table_id   int(11)
                // 1: chance  double
                // 2: item_template   int(11)

                int table = (int)row[0];
                double chance = (double)row[1];
                uint item_template = (uint)row[2];

                LootTable lt = _server.GetLootTable(table);
                if (lt == null)
                {
                    lt = new LootTable();
                    _server.AddLootTable(table, lt);                    
                }
                lt.AddEntry(chance, item_template);
            }

            t.Type = Task.TaskType.LoadLocations_Process;
            AddDBQuery("SELECT * FROM locations;", t);
        }

        void LoadLocations_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                Location loc = Location.ReadFromDB(row);
                _server.AddLocation(loc);
            }

            t.Type = Task.TaskType.LoadMonsterTemplates_Process;
            AddDBQuery("SELECT * FROM monster_templates;", t);
        }

        void LoadMonsterTemplates_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                MonsterTemplate mt = MonsterTemplate.ReadFromDB(row);
                _server.AddMonsterTemplate(mt);
            }

            t.Type = Task.TaskType.LoadMonsterSpawners_Process;
            AddDBQuery("SELECT * FROM monster_spawners;", t);
        }

        void LoadMonsterSpawners_Process_Handler(Task t)
        {
            foreach (object[] row in t.Query.Rows)
            {
                MonsterSpawner ms = MonsterSpawner.ReadFromDB(row);
                _server.AddMonsterSpawner(ms);
            }

            t.Type = Task.TaskType.LoadQuestLines_Process;
            AddDBQuery("SELECT * FROM quest_lines;", t);
        }

        void LoadQuestLines_Process_Handler(Task t)
        {
            if (t.Query.Rows.Count > 0)
            {
                // Get the lines from the database
                List<QuestLine> lines = new List<QuestLine>();
                foreach (object[] row in t.Query.Rows)
                {
                    QuestLine line = QuestLine.ReadFromDB(row);
                    lines.Add(line);
                }

                t.Type = Task.TaskType.LoadQuestSteps_Process;
                t.Args = lines.ToArray();
                AddDBQuery("SELECT * FROM quest_steps;", t);
            }
            else
            {
                LogInterface.Log("Database does not contain any quest lines. No quests will be available", LogInterface.LogMessageType.Game, true);
            }
        }

        void LoadQuestSteps_Process_Handler(Task t)
        {
            if (t.Query.Rows.Count > 0)
            {
                Dictionary<ulong, QuestStep.Builder> steps = new Dictionary<ulong, QuestStep.Builder>();
                foreach (object[] row in t.Query.Rows)
                {
                    // 0: quest_id      int(10) unsigned
                    // 1: step          tinyint(3) unsigned
                    // 2: type          tinyint(3) unsigned
                    // 3: count         int(10) unsigned
                    // 4: target_id     int(10) unsigned
                    // 5: owner_id      int(10) unsigned
                    uint quest = (uint)row[0];
                    byte step = (byte)row[1];
                    byte type = (byte)row[2];
                    uint count = row[3] == null ? 0 : (uint)row[3];
                    uint target = row[4] == null ? (uint)0 : (uint)row[4];
                    uint owner = (uint)row[5];

                    ulong key = ((ulong)step << 32) | quest;
                    steps[key] = new QuestStep.Builder(quest, step, owner, (QuestStep.CompletionType)type, count, target);
                }

                // Put all the lines into the steps
                QuestLine[] lines = (QuestLine[])t.Args;
                foreach (QuestLine line in lines)
                {
                    ulong key = ((ulong)line.StepNumber << 32) | line.QuestID;
                    steps[key].AddLine(line);
                }

                // Now go gather all the rewards
                t.Args = steps;
                t.Type = Task.TaskType.LoadQuestRewards_Process;
                AddDBQuery("SELECT * FROM quest_rewards;", t);
            }
            else
            {
                LogInterface.Log("Database does not contain any quest steps. No quests will be available", LogInterface.LogMessageType.Game, true);
            }
        }

        void LoadQuestRewards_Process_Handler(Task t)
        {
            if (t.Query.Rows.Count > 0)
            {
                Dictionary<ulong, QuestStep.Builder> steps = (Dictionary<ulong, QuestStep.Builder>)t.Args;

                // read and consolidate rewards
                foreach (object[] row in t.Query.Rows)
                {
                    // 0: quest_id    int(10) unsigned
                    // 1: step    tinyint(3) unsigned
                    // 2: type    tinyint(10) unsigned
                    // 3: context int(10) unsigned

                    uint quest = (uint)row[0];
                    byte step = (byte)row[1];
                    byte type = (byte)row[2];
                    uint context = (uint)row[3];

                    ulong key = ((ulong)step << 32) | quest;

                    steps[key].AddReward(new QuestReward((QuestReward.RewardType)type, context));
                }                

                // now build all the quest steps
                Dictionary<uint, Quest.Builder> questBuilders = new Dictionary<uint, Quest.Builder>();
                foreach (QuestStep.Builder qsb in steps.Values)
                {
                    QuestStep step = qsb.Build();
                    if (!questBuilders.ContainsKey(step.QuestID))
                        questBuilders[step.QuestID] = new Quest.Builder(step.QuestID);
                    questBuilders[step.QuestID].AddStep(step);
                }

                // now build all the quests
                Dictionary<uint, Quest> quests = new Dictionary<uint, Quest>();
                foreach (KeyValuePair<uint, Quest.Builder> kvp in questBuilders)
                {
                    Quest q = kvp.Value.Build();
                    quests[kvp.Key] = q;
                }

                // Give it to the server
                _server.QuestsLoaded(quests);
                
                // Load the quest info
                AddDBQuery("SELECT * FROM quest_requirements;", new Task(Task.TaskType.LoadQuestRequirements_Process));
            }
            else
            {
                LogInterface.Log("Database does not contain any quest rewards. No quests will be available", LogInterface.LogMessageType.Game, true);
            }
        }

        void LoadQuestRequirements_Process_Handler(Task t)
        {
            if (t.Query.Rows.Count > 0)
            {
                foreach (object[] row in t.Query.Rows)
                {
                    uint questID = (uint)row[0];
                    _server.AddQuestRequirement(questID, QuestRequirement.LoadFromDB(row));
                }
            }
            else
            {
            }

            // Now load the quest info
            AddDBQuery("SELECT * FROM quest_info;", new Task(Task.TaskType.LoadQuestInfo_Process));
        }

        void LoadQuestInfo_Process_Handler(Task t)
        {
            if (t.Query.Rows.Count > 0)
            {
                foreach (object[] row in t.Query.Rows)
                {
                    // 0: quest_id      int(10) unsigned
                    // 1: giver_id      int(10) unsigned
                    // 2: giver_map_id  smallint(5) unsigned

                    uint questID = (uint)row[0];
                    uint giver = (uint)row[1];
                    ushort map = (ushort)row[2];
                    _server.SetQuestGiver(questID, giver, map);
                }
            }
            else
            {
                LogInterface.Log("Database does not contain any quest info. No quests will be available", LogInterface.LogMessageType.Game, true);
            }
            _server.AllowConnections();
        }
        #endregion

        #region TaskHandlers
        void LoginRequest_Fetch_Handler(Task t)
        {
            LoginRequestPacket lrp = (LoginRequestPacket)t.Args;
            string sql = string.Format("SELECT * FROM accounts WHERE user_name=\"{0}\";", lrp.UserName);
            t.Type = Task.TaskType.LoginRequest_Process;
            AddDBQuery(sql, t);
        }

        void LoginRequest_Process_Handler(Task t)
        {
            LoginRequestPacket lrp = (LoginRequestPacket)t.Args;
            if (t.Query.Rows.Count > 0)
            {
                // 0: account_id
                // 1: user_name
                // 2: password
                object[] row = t.Query.Rows[0];
                int account_id = (int)row[0];
                string pass = (string)row[1];
                if (pass != lrp.Password)
                    t.Client.SendPacket(new LoginErrorPacket(LoginErrorPacket.ErrorCodes.PasswordDoesntMatch, "wrong password"));
                else
                {
                    // Login ok - TODO: Implement other server support
                    string authKey = _server.ExpectConnection(account_id);
                    t.Client.SendPacket(new ServerListPacket(_server.PlayServers, authKey));
                }
            }
            else
            {
                // Account not found
                t.Client.SendPacket(new LoginErrorPacket(LoginErrorPacket.ErrorCodes.AccountDoesntExist, "makeanaccount.com"));
            }
        }

        void CharacterList_Fetch_Handler(Task t)
        {
            CharacterListRequestPacket clrp = (CharacterListRequestPacket)t.Args;
            int expectedAccount = _server.GetExpectedConnection(clrp.AuthKey);
            if (expectedAccount <= 0)
            {
                // Unexpected, kill the connection
                t.Client.SendPacket(new ErrorMessagePacket("Unauthorized access attempt. The police have been notified!"));
                t.Client.Disconnect();
            }
            else
            {
                t.Client.AccountID = expectedAccount;
                string sql = string.Format("SELECT * FROM characters WHERE account_id={0};", expectedAccount);
                t.Type = Task.TaskType.CharacterList_Process;
                AddDBQuery(sql, t);

                // Send login response
                t.Client.SendPacket(new LoginResponsePacket(clrp.AuthKey));
            }
        }

        void CharacterList_Process_Handler(Task t)
        {
            CharacterSelectInfo[] slots = new CharacterSelectInfo[3];
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < t.Query.Rows.Count)
                {
                    slots[i] = new CharacterSelectInfo(t.Query.Rows[i]);
                }
                else
                    slots[i] = new CharacterSelectInfo();
            }

            t.Client.SetCharacterIDs(slots);
            t.Client.SendPacket(new CharacterListPacket(slots));
        }

        void CreateCharacter_Handler(Task t)
        {
            CreateCharacterPacket ccp = (CreateCharacterPacket)t.Args;
            bool millena = Utils.NationFromModelInfo(ccp.ModelInfo);            
            string sql = string.Format("INSERT INTO characters SET account_id={0},name=\"{1}\",model_info={2},job={3}; SELECT LAST_INSERT_ID();", t.Client.AccountID, ccp.Name, ccp.ModelInfo, millena ? 0 : 1);
            t.Type = Task.TaskType.CreateCharacter_Finish;
            AddDBQuery(sql, t);
            
            t.Client.SendEmptyPacket(0x3);
        }

        void CreateCharacter_Finish_Handler(Task t)
        {
            ulong result = t.Query.Rows.Count > 0 ? (ulong)t.Query.Rows[0][0] : 0;
            if (result > 0)
            {
                int id = (int)result;

                // Add Skills
                CreateCharacterPacket ccp = (CreateCharacterPacket)t.Args;
                bool millena = Utils.NationFromModelInfo(ccp.ModelInfo);
                string sql = string.Format("INSERT INTO char_skills SET character_id={0},skill_id={1};", id, millena ? 4996 : 4997);
                sql += string.Format("INSERT INTO char_skills SET character_id={0},skill_id={1};", id, millena ? 4998 : 4999);
                sql += string.Format("INSERT INTO characters_toolbar SET character_id={0};", id);
                AddDBQuery(sql, null, false);
            }
        }

        void DeleteCharacter_Handler(Task t)
        {
            CharacterNameClass dcp = (CharacterNameClass)t.Args;
            int id = t.Client.GetCharacterID(dcp.Name);
            string sql;
            if (id < 0)
                sql = string.Format("DELETE FROM characters WHERE account_id={0} AND name=\"{1}\";", t.Client.AccountID, dcp.Name);
            else
            {
                sql = string.Format("DELETE FROM characters WHERE character_id={0};", id);
                sql += string.Format("DELETE FROM char_skills WHERE character_id={0};", id);
                sql += string.Format("DELETE FROM characters_toolbar WHERE character_id={0};", id);
            }
            AddDBQuery(sql, null, false);

            t.Client.SendPacket(new DeleteCharacterConfirmPacket());
        }

        void SelectCharacter_Handler(Task t)
        {
            CharacterNameClass cnc = (CharacterNameClass)t.Args;
            int ID = t.Client.SelectCharacter(cnc.Name);
            t.Client.SendPacket(new SelectCharacterIDPacket(ID));
        }

        void SelectedCharacter_Fetch_Handler(Task t)
        {
            CharacterInfo ci = t.Client.LoadSelectedCharacter();
            int id = ci.ID;

            string sql = string.Format("SELECT * FROM characters_hv WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterDataHV_Process, t.Client, ci));
            ci.ExpectingHV = true;

            sql = string.Format("SELECT * FROM characters_lv WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterDataLV_Process, t.Client, ci));
            ci.ExpectingLV = true;

            sql = string.Format("SELECT * FROM item_instances WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterDataItems_Process, t.Client, ci));
            ci.ExpectingItems = true;

            sql = string.Format("SELECT * FROM char_skills WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterSkills_Process, t.Client, ci));
            ci.ExpectingSkills = true;

            sql = string.Format("SELECT * FROM active_quests WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterActiveQuests_Process, t.Client, ci));

            sql = string.Format("SELECT * FROM completed_quests WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterCompletedQuests_Process, t.Client, ci));

            sql = string.Format("SELECT * FROM characters_toolbar WHERE character_id={0};", id);
            AddDBQuery(sql, new Task(Task.TaskType.CharacterToolbar_Process, t.Client, ci));
        }

        void CharacterDataHV_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            if (t.Query.Rows.Count > 0)
            {
                ci.ReadHV(t.Query);
            }
            else
            {
                // Database doesnt have hv info for this character, put in defaults
                string sql = ci.SetHVDefaults();
                AddDBQuery(sql, null, false);
            }

            if (ci.LoadComplete)
            {
                t.Client.SendPacket(new MapChangePacket(ci));
            }
        }

        void CharacterDataLV_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            if (t.Query.Rows.Count > 0)
            {
                ci.ReadLV(t.Query);
            }
            else
            {
                // Database doesnt have lv info for this character, put in defaults
                string sql = ci.SetLVDefaults();
                AddDBQuery(sql, null, false);
            }

            if (ci.FrontierID > 0)
            {
                // get the frontier data
                string sql = string.Format("SELECT * FROM frontiers WHERE frontier_id={0};", ci.FrontierID);
                AddDBQuery(sql, new Task(Task.TaskType.CharacterFrontierData_Process, t.Client, ci));
                ci.ExpectingFrontier = true;
            }

            if (ci.LoadComplete)
            {
                t.Client.SendPacket(new MapChangePacket(ci));
            }
        }

        void CharacterDataItems_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            ci.ReadItems(t.Query);

            if (ci.LoadComplete)
            {
                t.Client.SendPacket(new MapChangePacket(ci));
            }
        }

        void CharacterFrontierData_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            ci.ReadFrontierData(t.Query);

            if (ci.LoadComplete)
            {
                t.Client.SendPacket(new MapChangePacket(ci));
            }
        }

        void CharacterSkills_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            ci.ReadSkills(t.Query);

            if (ci.LoadComplete)
            {
                t.Client.SendPacket(new MapChangePacket(ci));
            }
        }

        void CharacterActiveQuests_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            ci.ReadActiveQuests(t.Query, t.Client);
        }

        void CharacterCompletedQuests_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            ci.ReadCompletedQuests(t.Query);
        }

        void CharacterToolbar_Process_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            CharacterToolbar tb = new CharacterToolbar();
            if (t.Query.Rows.Count > 0)
            {
                tb.ReadFromDB(t.Query.Rows[0]);
                tb.SendToClient(t.Client, false);
            }
            ci.Toolbar = tb;
        }

        void CharacterActiveQuest_Save_Handler(Task t)
        {
            ActiveQuestArgs rqa = (ActiveQuestArgs)t.Args;

            string sql;
            if (rqa.Remove)
                sql = string.Format("DELETE FROM active_quests WHERE character_id={0} AND quest_id={1};", rqa.CharacterID, rqa.QuestID);
            else
                sql = string.Format("INSERT INTO active_quests (character_id,quest_id,step) VALUES ({0},{1},{2}) ON DUPLICATE KEY UPDATE step={2};", rqa.CharacterID, rqa.QuestID, rqa.Step);
            AddDBQuery(sql, null, false);

            if (rqa.Finished)
            {
                sql = string.Format("INSERT INTO completed_quests SET character_id={0},quest_id={1};", rqa.CharacterID, rqa.QuestID);
                AddDBQuery(sql, null, false);
            }             
        }

        void PlayerEnterMap_Handler(Task t)
        {
            _server.PlayerEnterMap(t.Client);
        }

        void PlayerMove_Handler(Task t)
        {
            // For now just allow the move for the client
            _server.ProcessMoveRequest(t.Client, (CharacterPositionClass)t.Args);
        }

        void PlayerUpdatePosition_Handler(Task t)
        {
            _server.UpdatePlayerPosition(t.Client, (CharacterPositionClass)t.Args);
        }

        void NPCDialogNextButton_Handler(Task t)
        {
            _server.NextDialog(t.Client);
        }

        void GiveGoldExpFame_Handler(Task t)
        {
            GiveGoldExpFameArgs args = (GiveGoldExpFameArgs)t.Args;
            CharacterInfo ci = t.Client.Character;
            ci.AddGoldExpFame(args.Gold, args.Exp, args.Fame);

            string sql = string.Format("UPDATE characters_hv SET gold={0},exp={1},fame={2} WHERE character_id={3};", ci.Gold, ci.Exp, ci.Fame, ci.ID);
            AddDBQuery(sql, null, false);

            string log = string.Format("Giving {0} gold, {1} exp, and {2} fame to Character ID: {3}. Reason: {4}, Context: {5}", args.Gold, args.Exp, args.Fame, ci.ID, args.Reason, args.Context);
            LogInterface.Log(log, LogInterface.LogMessageType.Game);

            // TODO: Figure out how to send this to the client
        }

        void GiveItem_Handler(Task t)
        {
            GiveItemArgs args = (GiveItemArgs)t.Args;
            CharacterInfo ci = t.Client.Character;

            // Instantiate the item
            Item item = _server.InstantiateItem(args.ItemTemplateID);
            
            // Log it
            string log = string.Format("Giving item template({0}) to Character ID: {1}. Reason: {2}, Context: {3}", args.ItemTemplateID, ci.ID, args.Reason, args.Context);
            LogInterface.Log(log, LogInterface.LogMessageType.Game);

            // Do autostacking
            if (item.ItemType == Item.Type.Stackable)
            {
                // Find all existing stacks with this model id
                Item[] stacks = ci.FindItemsByModel(item.Model);
                if (stacks.Length > 0)
                {
                    foreach (Item stack in stacks)
                    {
                        if (stack.StackSpace > 0)
                        {
                            // There is room in this stack, put items into this stack until full
                            int amountToAdd = Math.Min(stack.StackSpace, item.Quantity);

                            // Change this stacks quantity, save in the database, and send to the client
                            stack.AddQuantity(amountToAdd);
                            AddDBQuery(stack.UpdateDBString(), null, false);
                            if( item.Quantity > amountToAdd )
                                t.Client.SendPacket(new GiveItemPacket(stack));

                            // Remove the quantity from the new item
                            item.AddQuantity(-amountToAdd);
                            if (item.Quantity <= 0)
                            {
                                // All of the quantity has been distributed
                                // Tell the client about the loot with the last stack
                                t.Client.SendPacket(new ItemLootPacket(stack, args.Context));
                                return;     
                            }
                        }
                    }
                }
            }

            // Find a slot for it
            item.Slot = ci.FindGeneralSlot();

            // Save it in the database
            string sql = item.WriteDBString(args.ItemTemplateID, ci.ID);
            t.Type = Task.TaskType.GiveItem_Finish;
            args.Item = item;
            AddDBQuery(sql, t);

        }
        
        void GiveItem_Finish_Handler(Task t)
        {
            GiveItemArgs args = (GiveItemArgs)t.Args;
            Item item = args.Item;
            ulong res = (ulong)t.Query.Rows[0][0];
            item.ID = (uint)res;
            
            // Store it in the character
            t.Client.Character.AddItem(item);

            // Tell the client about it
            if( args.Reason == GiveItemArgs.TheReason.Loot )
                t.Client.SendPacket(new ItemLootPacket(item, args.Context));
            else
                t.Client.SendPacket(new GiveItemPacket(item));
        }

        void RemoveCharacter_Handler(Task t)
        {
            CharacterInfo ci = (CharacterInfo)t.Args;
            _server.RemoveCharacterFromMap(ci);
        }

        void GMCommand_Process_Handler(Task t)
        {
            GMCommandPacket cmd = (GMCommandPacket)t.Args;

            string cmdString = string.Format("GM Command - {0} ({1}, {2}, {3}, {4}); Character: {5}", cmd.Command, cmd.Param, cmd.X, cmd.Y, cmd.Param2, cmd.Character);
            LogInterface.Log(cmdString);

            switch (cmd.Command)
            {
                case 0xfa3:
                    AddTask(new Task(Task.TaskType.GiveItem, t.Client,new GiveItemArgs((uint)cmd.Param, GiveItemArgs.TheReason.GMCommand, 0)));
                    break;
                default:
                    //LogInterface.Log("Unhandled GM Command: 0x" + cmd.Command.ToString("x"));
                    break;
            }
        }

        void MoveItem_Handler(Task t)
        {
            MoveItemRequest mir = (MoveItemRequest)t.Args;
            bool success = true;
            if (mir.OtherID != 0)
            {
                // Swap positions with the other item
                Item item = t.Client.Character.FindItem(mir.ItemID);
                Item other = t.Client.Character.FindItem(mir.OtherID);
                if (item == null || other == null)
                {
                    success = false;
                }
                else
                {
                    byte tempSlot = other.Slot;
                    other.Slot = item.Slot;
                    item.Slot = tempSlot;

                    AddDBQuery(item.UpdateDBString(), null, false);
                    AddDBQuery(other.UpdateDBString(), null, false);
                }
            }
            else
            {
                // Just move this object to the new slot
                Item item = t.Client.Character.FindItem(mir.ItemID);
                if (item == null)
                {
                    success = false;
                }
                else
                {
                    item.Slot = mir.Slot;

                    AddDBQuery(item.UpdateDBString(), null, false);
                }
            }
            t.Client.SendPacket(new MoveItemResponse(mir.ItemID, mir.OtherID, mir.Slot, success));            
        }

        void EquipItem_Handler(Task t)
        {
            EquipItemRequest eir = (EquipItemRequest)t.Args;
            Item item = t.Client.Character.FindItem(eir.ItemID);
            Item equipped = t.Client.Character.EquippedItem(eir.Slot);
            bool visible = false;
            if (equipped != null)
            {
                t.Client.Character.UnEquipItem(eir.Slot);
                AddDBQuery(equipped.UpdateDBString(), null, false);
                visible = true;
            }

            if (item != null)
            {
                t.Client.Character.EquipItem(item, eir.Slot);
                AddDBQuery(item.UpdateDBString(), null, false);
                visible = true;
                t.Client.NotifyEquipItem();
            }

            t.Client.SendPacket(new EquipItemResponse(t.Client.Character, item, equipped, visible));

            if (visible)
            {
                // Show all nearby clients the change
                SeeEquipmentChangePacket pkt = new SeeEquipmentChangePacket(t.Client.Character.ID, item);
                PlayMap map = _server.GetPlayMap(t.Client.Character.MapID);
                Connection[] players = map.Players;
                foreach (Connection c in players)
                {
                    if (c != t.Client)
                    {
                        c.SendPacket(pkt);
                    }
                }
            }
        }

        void UnEquipItem_Handler(Task t)
        {
            byte equipSlot = 0xFF;

            EquipItemRequest eir = (EquipItemRequest)t.Args;
            Item equipped = t.Client.Character.EquippedItem(eir.ItemID);
            if (equipped != null)
            {
                equipSlot = equipped.Slot;
                t.Client.Character.UnEquipItem(equipped.Slot, eir.Slot);
                AddDBQuery(equipped.UpdateDBString(), null, false);
            }

            // Send response to the client
            t.Client.SendPacket(new UnEquipItemResponse(t.Client.Character, eir.ItemID, eir.Slot, equipSlot != 0xFF));

            if (equipSlot != 0xFF)
            {
                // Tell other players about it
                SeeUnequipPacket pkt = new SeeUnequipPacket(t.Client.Character.ID, equipSlot);
                PlayMap map = _server.GetPlayMap(t.Client.Character.MapID);
                Connection[] players = map.Players;
                foreach (Connection c in players)
                {
                    if (c != t.Client)
                    {
                        c.SendPacket(pkt);
                    }
                }
            }
        }

        void Teleport_Handler(Task t)
        {
            // Move the character
            t.Client.Character.Teleport((Location)t.Args);

            // Tell the client
            t.Client.SendPacket(new MapChangePacket(t.Client.Character));
        }

        void UpdateNPCPosition_Handler(Task t)
        {
            NPC npc = (NPC)t.Args;
            PlayMap map = _server.GetPlayMap(npc.MapID);
            map.UpdateNPCPosition(npc);
        }

        void DoAttack_Handler(Task t)
        {
            AttackTargetRequest atr = (AttackTargetRequest)t.Args;
            PlayMap map = _server.GetPlayMap(t.Client.Character.MapID);
            if( atr.TargetT == AttackTargetRequest.TargetType.Monster )
                map.PlayerAttackMonster(t.Client, atr);
        }

        void MonsterAttackPlayer_Handler(Task t)
        {
            object[] args = (object[])t.Args;
            Monster m = (Monster)args[0];
            int damage = (int)args[1];
            ushort attackType = (ushort)args[2];

            t.Client.Character.TakeDamage(damage);

            PlayerGetAttackedPacket pkt = new PlayerGetAttackedPacket(m, t.Client.Character, attackType);
            t.Client.SendPacket(pkt);
        }

        void UseItem_Handler(Task t)
        {
            Item.ItemError err = Item.ItemError.None;
            uint item = (uint)t.Args;
            Item stack = t.Client.Character.FindItem(item);
            if (stack != null)
            {
                err = stack.Use(t.Client);
                if (stack.Quantity <= 0)
                {
                    // Stack is gone, delete the item
                    string sql = string.Format("DELETE FROM item_instances WHERE instance_id={0};", stack.ID);
                    AddDBQuery(sql, null, false);
                }
                else
                {
                    // Update the stack in the database and the client
                    AddDBQuery(stack.UpdateDBString(), null, false);
                    t.Client.SendPacket(new GiveItemPacket(stack));
                }

                t.Client.SendPacket(new UseItemResponse(stack.ID, (byte)stack.Quantity, err));
            }
            else
                t.Client.SendPacket(new UseItemResponse(0, 0, Item.ItemError.UnableToFindItem));
        }

        void ToolbarItemSet_Handler(Task t)
        {
            ToolbarItemSetRequest tbr = (ToolbarItemSetRequest)t.Args;

            string sql = t.Client.Character.Toolbar.SetItem(t.Client, tbr);            
            AddDBQuery(sql, null, false);
        }

        void ToolbarItemClear_Handler(Task t)
        {
            string sql = t.Client.Character.Toolbar.ClearItem(t.Client, (byte)t.Args);
            AddDBQuery(sql, null, false);
        }
        #endregion
    }
}
