using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
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

            string sql = string.Format("SELECT * FROM static_npcs WHERE map_id={0};", mapID);
            List<object[]> rows = ExecuteQuery(sql);
            foreach (object[] row in rows)
            {
                npcs.Add(new NPC((uint)row[0], (ushort)row[1], (uint)row[2], (uint)row[3], (uint)row[4], (uint)row[5]));
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
            string sql = string.Format("INSERT INTO static_npcs SET game_id={0},location_x={1},location_y={2},map_id={3}; SELECT LAST_INSERT_ID();", 0, x, y, mapId);
            List<object[]> rows = ExecuteQuery(sql);
            
            ulong id = (ulong)rows[0][0];
            NPC npc = new NPC((uint)id, 0, (uint)x, (uint)y, 10000, 0);
            return npc;
        }

        static public void DeleteNPC(NPC npc)
        {
            string sql = string.Format("DELETE FROM static_npcs WHERE static_npc_id={0};", npc.ID);
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
        public bool Dirty;

        public NPC(uint id, ushort gameID, uint x, uint y, uint hp, uint direction)
        {
            ID = id;
            GameID = gameID;
            X = x;
            Y = y;
            HP = hp;
            Direction = direction;
            Dirty = false;
        }

        public override string ToString()
        {
            return ID.ToString() + ": " + GameID.ToString();
        }
    }
}
