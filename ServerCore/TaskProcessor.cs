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
            LoadQuestLines_Process,
            LoadQuestSteps_Process,
            LoadQuestRewards_Process,
            LoadQuestInfo_Process,

            LoginRequest_Fetch,
            LoginRequest_Process,
            CharacterList_Fetch,
            CharacterList_Process,
            CreateCharacter,
            DeleteCharacter,
            SelectCharacter,
            SelectedCharacter_Fetch,
            CharacterDataHV_Process,
            CharacterDataLV_Process,
            CharacterDataItems_Process,
            CharacterFrontierData_Process,
            CharacterSkills_Process,
            PlayerEnterMap,
            PlayerMove,
            PlayerUpdatePosition,
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
            _taskHandlers[Task.TaskType.LoadQuestLines_Process] = LoadQuestLines_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestSteps_Process] = LoadQuestSteps_Process_Handler;
            _taskHandlers[Task.TaskType.LoginRequest_Fetch] = LoginRequest_Fetch_Handler;
            _taskHandlers[Task.TaskType.LoginRequest_Process] = LoginRequest_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestRewards_Process] = LoadQuestRewards_Process_Handler;
            _taskHandlers[Task.TaskType.LoadQuestInfo_Process] = LoadQuestInfo_Process_Handler;

            _taskHandlers[Task.TaskType.CharacterList_Fetch] = CharacterList_Fetch_Handler;
            _taskHandlers[Task.TaskType.CharacterList_Process] = CharacterList_Process_Handler;
            _taskHandlers[Task.TaskType.CreateCharacter] = CreateCharacter_Handler;
            _taskHandlers[Task.TaskType.DeleteCharacter] = DeleteCharacter_Handler;
            _taskHandlers[Task.TaskType.SelectCharacter] = SelectCharacter_Handler;
            _taskHandlers[Task.TaskType.SelectedCharacter_Fetch] = SelectedCharacter_Fetch_Handler;
            _taskHandlers[Task.TaskType.CharacterDataHV_Process] = CharacterDataHV_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterDataLV_Process] = CharacterDataLV_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterDataItems_Process] = CharacterDataItems_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterFrontierData_Process] = CharacterFrontierData_Process_Handler;
            _taskHandlers[Task.TaskType.CharacterSkills_Process] = CharacterSkills_Process_Handler;
            _taskHandlers[Task.TaskType.PlayerEnterMap] = PlayerEnterMap_Handler;
            _taskHandlers[Task.TaskType.PlayerMove] = PlayerMove_Handler;
            _taskHandlers[Task.TaskType.PlayerUpdatePosition] = PlayerUpdatePosition_Handler;


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
                    // 4: target_id     smallint(5) unsigned
                    uint quest = (uint)row[0];
                    byte step = (byte)row[1];
                    byte type = (byte)row[2];
                    uint count = row[3] == null ? 0 : (uint)row[3];
                    ushort target = row[4] == null ? (ushort)0 : (ushort)row[4];

                    ulong key = ((ulong)step << 32) | quest;
                    steps[key] = new QuestStep.Builder(quest, step, (QuestStep.CompletionType)type, count, target);
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
                // read and consolidate rewards & prewards
                Dictionary<ulong, QuestReward.Builder> rewards = new Dictionary<ulong, QuestReward.Builder>();
                Dictionary<ulong, QuestReward.Builder> prewards = new Dictionary<ulong, QuestReward.Builder>();
                foreach (object[] row in t.Query.Rows)
                {
                    // 0: quest_id  int(10) unsigned
                    // 1: step      tinyint(3) unsigned
                    // 2: gold      int(10) unsigned
                    // 3: exp       int(10) unsigned
                    // 4: item      smallint(5) unsigned
                    // 5: preward	tinyint(3) unsigned

                    uint quest = (uint)row[0];
                    byte step = (byte)row[1];
                    uint gold = (uint)row[2];
                    uint exp = (uint)row[3];
                    ushort item = (ushort)row[4];
                    byte preward = (byte)row[5];

                    ulong key = ((ulong)step << 32) | quest;
                    Dictionary<ulong, QuestReward.Builder> rewardDict = (preward != 0) ? prewards : rewards;

                    if (!rewardDict.ContainsKey(key))
                        rewardDict[key] = new QuestReward.Builder(0, 0);

                    if (item != 0)
                        rewardDict[key].AddItem(item);
                    if (gold != 0)
                        rewardDict[key].AddGold(gold);
                    if (exp != 0)
                        rewardDict[key].AddExp(exp);
                }

                // Put the rewards into the appropriate steps
                Dictionary<ulong, QuestStep.Builder> steps = (Dictionary<ulong, QuestStep.Builder>)t.Args;
                foreach (KeyValuePair<ulong, QuestReward.Builder> kvp in rewards)
                {
                    steps[kvp.Key].AddReward(kvp.Value.Build());
                }

                // Put the prewards into the appropriate steps
                foreach (KeyValuePair<ulong, QuestReward.Builder> kvp in prewards)
                {
                    steps[kvp.Key].AddPreward(kvp.Value.Build());
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
                AddDBQuery("SELECT * FROM quest_info;", new Task(Task.TaskType.LoadQuestInfo_Process));
            }
            else
            {
                LogInterface.Log("Database does not contain any quest rewards. No quests will be available", LogInterface.LogMessageType.Game, true);
            }
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
            string sql = string.Format("INSERT INTO characters SET account_id={0},name=\"{1}\",model_info={2},job={3};", t.Client.AccountID, ccp.Name, ccp.ModelInfo, millena ? 0 : 1);
            AddDBQuery(sql, null, false);

            t.Client.SendEmptyPacket(0x3);
        }

        void DeleteCharacter_Handler(Task t)
        {
            CharacterNameClass dcp = (CharacterNameClass)t.Args;
            int id = t.Client.GetCharacterID(dcp.Name);
            string sql;
            if( id < 0 )
                sql = string.Format("DELETE FROM characters WHERE account_id={0} AND name=\"{1}\";", t.Client.AccountID, dcp.Name);
            else
                sql = string.Format("DELETE FROM characters WHERE character_id={0};", id);
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
        #endregion
    }
}
