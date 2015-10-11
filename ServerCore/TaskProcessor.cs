using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DecoServer2.CharacterThings;

namespace JuggleServerCore
{    
    public class Task
    {
        public enum TaskType
        {
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
            _taskHandlers[Task.TaskType.LoginRequest_Fetch] = LoginRequest_Fetch_Handler;
            _taskHandlers[Task.TaskType.LoginRequest_Process] = LoginRequest_Process_Handler;
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


            _pendingQueries = new Dictionary<long, Task>();
            _pqLock = new Mutex();
        }

        public void Database_OnQueryComplete(object sender, EventArgs e)
        {
            DBQuery q = (DBQuery)sender;

            _pqLock.WaitOne();
            LogInterface.Log("Finishing Query with key: " + q.Key, LogInterface.LogMessageType.Debug, true);
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
        #endregion
    }
}

/*
name : (S - C)  Opcode : 0015, Data : 
                                      5069          00000: 00ED012A 00030000 000E0100 00005069   ...* .... .... ..Pi
       6E6B0000 00000000 00000000 00000068          00010: 6E6B0000 00000000 00000000 00000000   nk.. .... .... ....
       69206576 65727962 6F647920 3A292900          00020: 00000000 00000000 00000000 00000000   .... .... .... ....
       00000000 00000000 00000000 00000AA1          00030: 00000000 00000000 00000000 000012A1   .... .... .... ....
       F84E0100 00000000 00000000 01010000          00040: F84E0100 00000000 00000000 01010000   .N.. .... .... ....
       00010000 00000000 00000700 DDBC0100          00050: 00010000 00000000 00000700 DDBC0100   .... .... .... ....
       96000000 96000000 96000000 00C0000A          00060: 96000000 96000000 96000000 00C0000A   .... .... .... ....
       000C0002 00050008 0A000C00 96000000          00070: 000C0002 00050008 0A000C00 96000000   .... .... .... ....
       96000000 96000000 0D000900 0D000C00          00080: 96000000 96000000 0D000900 0D000C00   .... .... .... ....
       0C000D00 00030000 00006400 00000000          00090: 0C000D00 00000000 00006400 00000000   .... .... ..d. ....
       00000000 00000000 00000000 00000100          000A0: 00000000 7B000000 00000000 00000000   .... {... .... ....
       01000003 00000101 01010101 017D0001          000B0: 00000000 00000101 01010101 01000000   .... .... .... ....
       00000001 00647361 64617300 00000000          000C0: 00000000 00000000 00000000 00000000   .... .... .... ....
       00000000 00000000 00000000 00000000          000D0: 00000000 00000000 00000000 00000000   .... .... .... ....
       00000000 00000000 00000000 00000000          000E0: 000000                                ...
       00000000 00000000 00000000 00000000 
       00000000 00000000 00000020 07036A2B 
       F4010000 6B2BF401 00006C2B F4010000 
       01000000 05686969 00000000 00000000 
       00000000 E8030000 31323334 35363738 
       39306162 6364 
        
       


*/