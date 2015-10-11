using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace JuggleServerCore
{
    public class ServerBase
    {
        DatabaseThread _db;
        ListenThread _lt;
        InputThread _inputThread;
        TaskProcessor _taskProcessor;

        List<PlayServerInfo> _playServers;

        Dictionary<string, int> _expectedConnections;

        public ServerBase(int listenPort, string dbConnectString)
        {
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

            // Start listen thread
            _lt = new ListenThread(listenPort);
            _lt.OnConnectionAccepted += _lt_OnConnectionAccepted;

            // Start input thread
            _inputThread = new InputThread();
        }

        public void Run()
        {
            // Allow connections
            _lt.Start();

            // Setup db callback
            if (_db != null)
            {
                _taskProcessor.Database = _db;
                _db.OnQueryComplete += new EventHandler(_taskProcessor.Database_OnQueryComplete);
            }

            // Process            
            _taskProcessor.Process();

            // Shutdown
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

        #endregion

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
        #endregion

    }
}
