using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DecoServer2.CharacterThings;
using DecoServer2;
using DecoServer2.Quests;

namespace JuggleServerCore
{
    public class ServerBase
    {
        DatabaseThread _db;
        ListenThread _lt;
        InputThread _inputThread;
        AIThread _aiThread;
        TaskProcessor _taskProcessor;
        int _listenPort;

        List<PlayServerInfo> _playServers;

        Dictionary<string, int> _expectedConnections;

        Random _rand;

        public ServerBase(int listenPort, string dbConnectString)
        {
            _rand = new Random();
            _playServers = new List<PlayServerInfo>();
            _expectedConnections = new Dictionary<string, int>();

            IPAddress[] IPS = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in IPS)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    _playServers.Add(new PlayServerInfo("Main Server", ip.ToString(), (ushort)listenPort));
            }


            // Start log thread
            LogThread log = new LogThread();

            // Start database thread
            if( dbConnectString != null )
                _db = new DatabaseThread(dbConnectString);

            // Start AI Thread
            _aiThread = new AIThread();

            _listenPort = listenPort;

            // Start input thread
            _inputThread = new InputThread();
        }

        public void Run()
        {
            // Setup db callback
            if (_db != null)
            {
                _taskProcessor.Database = _db;
                _db.OnQueryComplete += new EventHandler(_taskProcessor.Database_OnQueryComplete);
            }

            SetupWorld();

            // Process            
            _taskProcessor.Process();

            // Shutdown
        }

        // Called after server data is all loaded
        public void AllowConnections()
        {
            // Start listen thread
            if (_lt == null)
            {
                _lt = new ListenThread(_listenPort);
                _lt.OnConnectionAccepted += _lt_OnConnectionAccepted;

                // Allow connections
                _lt.Start();
            }
        }

        private void _lt_OnConnectionAccepted(object sender, SocketArg e)
        {
            Connection client = new Connection(e.Socket);

            client.OnLoginRequest += Client_OnLoginRequest;
            client.OnCharacterListRequest += Client_OnCharacterListRequest;
            client.OnCreateCharacter += Client_OnCharacterCreate;
            client.OnDeleteCharacter += Client_OnDeleteCharacter;
            client.OnSelectCharacter += Client_OnSelectCharacter;
            client.OnLoadSelectedCharacter += Client_OnLoadSelectedCharacter;
            client.OnPlayerEnterMap += Client_OnPlayerEnterMap;
            client.OnMoveTo += Client_OnMoveTo;
            client.OnUpdatePosition += Client_OnUpdatePosition;
            client.OnNPCDialogNextButton += Client_OnNPCDialogNextButton;
            client.OnGMCommand += Client_OnGMCommand;
            client.OnMoveItem += Client_OnMoveItem;
            client.OnToolbarItemSet += Client_OnToolbar;
            client.OnToolbarItemClear += Client_OnToolbarItemClear;
            client.OnUseItem += Client_OnUseItem;
            client.OnEquipItem += Client_OnEquipItem;
            client.OnUnEquipItem += Client_OnUnEquipItem;
            client.OnAttack += Client_OnAttack;

            InputThread.AddConnection(client);
        }

        #region Client Event Handlers
        private void Client_OnLoginRequest(object sender, LoginRequestPacket e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.LoginRequest_Fetch, (Connection)sender, e));
        }

        private void Client_OnCharacterListRequest(object sender, CharacterListRequestPacket e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.CharacterList_Fetch, (Connection)sender, e));
        }

        private void Client_OnCharacterCreate(object sender, CreateCharacterPacket e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.CreateCharacter, (Connection)sender, e));
        }

        private void Client_OnDeleteCharacter(object sender, CharacterNameClass e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.DeleteCharacter, (Connection)sender, e));
        }

        private void Client_OnSelectCharacter(object sender, CharacterNameClass e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.SelectCharacter, (Connection)sender, e));
        }

        private void Client_OnLoadSelectedCharacter(object sender, EventArgs e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.SelectedCharacter_Fetch, (Connection)sender, e));
        }

        private void Client_OnPlayerEnterMap(object sender, EventArgs e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.PlayerEnterMap, (Connection)sender));
        }
        
        private void Client_OnMoveTo(object sender, CharacterPositionClass e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.PlayerMove, (Connection)sender, e));
        }

        private void Client_OnUpdatePosition(object sender, CharacterPositionClass e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.PlayerUpdatePosition, (Connection)sender, e));
        }

        private void Client_OnNPCDialogNextButton(object sender, EventArgs e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.NPCDialogNextButton, (Connection)sender, null));
        }
        
        private void Client_OnGMCommand(object sender, GMCommandPacket e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.GMCommand_Process, (Connection)sender, e));
        }

        private void Client_OnMoveItem(object sender, MoveItemRequest e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.MoveItem, (Connection)sender, e));
        }

        private void Client_OnEquipItem(object sender, EquipItemRequest e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.EquipItem, (Connection)sender, e));
        }

        private void Client_OnUnEquipItem(object sender, EquipItemRequest e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.UnEquipItem, (Connection)sender, e));
        }

        private void Client_OnAttack(object sender, AttackTargetRequest e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.DoAttack, (Connection)sender, e));
        }

        private void Client_OnUseItem(Connection arg1, uint arg2)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.UseItem, arg1, arg2));
        }

        private void Client_OnToolbar(object sender, ToolbarItemSetRequest e)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.ToolbarItemSet, (Connection)sender, e));
        }

        private void Client_OnToolbarItemClear(Connection arg1, byte arg2)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.ToolbarItemClear, arg1, arg2));
        }
        #endregion

        public void RemoveCharacter(CharacterInfo ci)
        {
            TaskProcessor.AddTask(new Task(Task.TaskType.RemoveCharacter, null, ci));
        }

        public string ExpectConnection(int accountId)
        {
            Int64 code = accountId * DateTime.Now.Ticks;
            string authKey = "";
            
            do
            {
                authKey = "";
                for (int i = 0; i < 8; i++)
                {
                    byte p = (byte)((code >> (i * 8)) & 0xFF);
                    if (p != 0)
                        authKey += p.ToString("X2");
                }
            } while( _expectedConnections.ContainsKey(authKey));
            _expectedConnections[authKey] = accountId;
            LogInterface.Log(string.Format("Expecting client for account {0} with key {1}", accountId, authKey), LogInterface.LogMessageType.Security, false); 

            return authKey;
        }

        public int GetExpectedConnection(string authKey)
        {
            if( !_expectedConnections.ContainsKey(authKey) )
                return -1;

            int account = _expectedConnections[authKey];
            _expectedConnections.Remove(authKey);
            LogInterface.Log(string.Format("Got connection for account {0} with key {1}", account, authKey), LogInterface.LogMessageType.Security, false);
            return account;
        }

        #region World Stuff
        Dictionary<ushort, PlayMap> _maps;
        Dictionary<uint, ItemTemplate> _itemTemplates;
        Dictionary<uint, Location> _locations;
        Dictionary<uint, MonsterTemplate> _monsterTemplates;
        Dictionary<int, LootTable> _lootTables;
        Dictionary<uint, LevelData> _levelData;

        void SetupWorld()
        {
            _maps = new Dictionary<ushort, PlayMap>();
            _itemTemplates = new Dictionary<uint, ItemTemplate>();
            _locations = new Dictionary<uint, Location>();
            _monsterTemplates = new Dictionary<uint, MonsterTemplate>();
            _lootTables = new Dictionary<int, LootTable>();
            _levelData = new Dictionary<uint, LevelData>();
            TaskProcessor.AddTask(new Task(Task.TaskType.LoadPlayMaps_Fetch));
        }

        public void AddLevelData(LevelData ld)
        {
            _levelData[ld.Level] = ld;
        }

        public LevelData GetLevelData(uint level)
        {
            return _levelData[level];
        }

        public void AddLootTable(int table, LootTable lt)
        {
            _lootTables[table] = lt;
        }

        public LootTable GetLootTable(int table)
        {
            if( _lootTables.ContainsKey(table) )
                return _lootTables[table];
            return null;
        }

        public void AddLocation(Location loc)
        {
            _locations[loc.ID] = loc;
        }

        public Location GetLocation(uint id)
        {
            if( _locations.ContainsKey(id) )
                return _locations[id];
            return null;
        }

        public void AddMonsterTemplate(MonsterTemplate mt)
        {
            _monsterTemplates[mt.ID] = mt;
        }

        public MonsterTemplate GetMonsterTemplate(uint id)
        {
            if( _monsterTemplates.ContainsKey(id) )
                return _monsterTemplates[id];
            return null;
        }

        public void AddMonsterSpawner(MonsterSpawner ms)
        {
            _aiThread.AddSpawner(ms);
        }

        public void AddPlayMap(ushort mapID)
        {
            _maps[mapID] = new PlayMap(mapID);
        }

        public PlayMap GetPlayMap(ushort mapID)
        {
            return _maps[mapID];
        }

        public void AddNPC(NPC npc)
        {
            _maps[npc.MapID].AddNPC(npc);
        }

        public void PlayerEnterMap(Connection client)
        {
            CharacterInfo ci = client.Character;
            _maps[ci.MapID].AddPlayer(client, ci);
        }

        public void ProcessMoveRequest(Connection client, CharacterPositionClass mtp)
        {
            _maps[client.Character.MapID].ProcessMoveRequest(client, mtp);
        }

        public void UpdatePlayerPosition(Connection client, CharacterPositionClass cpc)
        {
            _maps[client.Character.MapID].UpdatePlayerPosition(client, cpc);
        }

        public void RemoveCharacterFromMap(CharacterInfo ci)
        {
            _maps[ci.MapID].RemovePlayer(ci);
        }
        #endregion

        #region Item Stuff
        public void AddItemTemplate(ItemTemplate it)
        {
            _itemTemplates[it.ID] = it;
        }

        public ItemTemplate GetItemTemplate(uint itemTemplateID)
        {
            return _itemTemplates[itemTemplateID];
        }

        public Item InstantiateItem(uint itemTemplateID)
        {
            return _itemTemplates[itemTemplateID].Instantiate();
        }
        #endregion

        #region Quest Stuff
        Dictionary<uint, Quest> _quests;
        public void QuestsLoaded(Dictionary<uint, Quest> quests)
        {
            _quests = quests;
            LogInterface.Log(string.Format("Loaded {0} quests", quests.Count), LogInterface.LogMessageType.Game, true);
        }

        public void SetQuestGiver(uint questID, uint giverID, ushort mapID)
        {
            _maps[mapID].SetQuestGiver(_quests[questID], giverID);
        }

        public void AddQuestRequirement(uint questID, QuestRequirement qr)
        {
            _quests[questID].AddRequirement(qr);
        }

        public Quest GetQuest(uint questID)
        {
            return _quests[questID];
        }

        public void NextDialog(Connection client)
        {
            _maps[client.Character.MapID].NextDialog(client);
        }
        #endregion

        #region Accessors
        public ListenThread ListenThread
        {
            get { return _lt; }
        }

        public InputThread InputThread
        {
            get { return _inputThread; }
        }

        public DatabaseThread Database
        {
            get { return _db; }
        }

        public TaskProcessor TaskProcessor
        {
            get { return _taskProcessor; }
            set { _taskProcessor = value; }
        }

        public PlayServerInfo[] PlayServers
        {
            get { return _playServers.ToArray(); }
        }

        public Random Rand
        {
            get { return _rand; }
        }
        #endregion

    }
}
