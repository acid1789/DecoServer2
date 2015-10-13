using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JuggleServerCore;
using DecoServer2.Quests;

namespace DecoServer2
{
    public class NPC
    {
        uint _id;
        ushort _gameID;
        uint _cellIndex;
        uint _hp;
        float _direction;
        ushort _mapID;

        Dictionary<uint, Quest> _quests;

        private NPC()
        {
            _quests = new Dictionary<uint, Quest>();
        }

        public void DoDialog(Connection client)
        {
            /*
            // Is the player working for me?
            foreach (Quest q in client.Quests)
            {
                if (q.IsNPCRelevent(_mapID, _gameID))
                {
                    client.SendPacket(new NPCDialogPacket(q.QuestText(_mapID, _gameID), q.QuestIcon(_mapID, _gameID));
                    return;
                }
            }
            */
            
            // Do I have something for the player?
            foreach (Quest q in _quests.Values)
            {/*
                if (q.PlayerIsQualified(client))
                {
                    client.RecieveQuest(q);
                    client.SendPacket(new NPCDialogPacket(q.QuestText(_mapID, _gameID), q.QuestIcon(_mapID, _gameID));
                    return;
                }
                */
            }

            /*
            // Am I selling something?
            if (IsMerchant)
            {
                // Show sell dialog
            }

            // Just say hello
            if (_defaultText != null)
            {
                client.SendPacket(new NPCDialogPacket(_defaultText, _defaultIcon));
            }
            */
        }

        public void AddQuest(Quest q)
        {            
            _quests[q.QuestID] = q;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(_id);
            bw.Write(_gameID);
            bw.Write(_cellIndex);
            bw.Write(_hp);
            bw.Write((byte)1);      // Unknown byte
            bw.Write((ushort)50);   // Unknown short
            bw.Write((byte)0x20);   // Flags byte 0x20 = just position data

            // Position data
            bw.Write((byte)0);      // Unknown byte
            bw.Write((uint)0);      // Unknown dword
            bw.Write(_direction);   // Direction in degrees
        }

        static public NPC ReadFromDB(object[] row)
        {
            // 0: static_npc_id   int(10) unsigned
            // 1: game_id smallint(5) unsigned
            // 2: location_x  int(10) unsigned
            // 3: location_y  int(10) unsigned
            // 4: hp  int(10) unsigned
            // 5: direction   int(10) unsigned
            // 6: map_id  smallint(5) unsigned

            NPC npc = new NPC();
            npc._id = (uint)row[0];
            npc._gameID = (ushort)row[1];
            uint x = (uint)row[2];
            uint y = (uint)row[3];
            npc._hp = (uint)row[4];
            npc._direction = ((uint)row[5]);
            npc._mapID = (ushort)row[6];
            npc._cellIndex = Utils.EncodeCellIndex(npc._mapID, x, y);
            
            return npc;            
        }

        #region Accessors
        public uint ID
        {
            get { return _id; }
        }

        public ushort GameID
        {
            get { return _gameID; }
        }

        public ushort MapID
        {
            get { return _mapID; }
        }

        public uint CellIndex
        {
            get { return _cellIndex; }
        }

        public ushort DataSize
        {
            get { return 27; }  // For now just going to do this since the write is hardcoded as well
        }
        #endregion
    }
}
