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
            // Is the player working for me?
            Quest playerActiveQuest = client.Character.GetActiveQuestForNPC(_id);
            if (playerActiveQuest != null)
            {
                client.SetCurrentQuest(_id, playerActiveQuest.QuestID, 0);
                QuestLine ql = playerActiveQuest.GetStep(0).GetLine(0);
                ql.SendToClient(client);
                client.SendPacket(new NPCDialogPacket(_gameID));
                return;
            }
            
            // Do I have something for the player?
            foreach (Quest q in _quests.Values)
            {
                if (!client.Character.HasActiveQuest(q.QuestID) && !client.Character.HasCompletedQuest(q.QuestID))
                {
                    if (q.PlayerMeetsRequirements(client.Character))
                    {
                        client.Character.ReceiveQuest(q);
                        client.SetCurrentQuest(_id, q.QuestID, 0);
                        QuestStep qs = q.GetStep(0);
                        qs.Activate(client);
                        QuestLine ql = qs.GetLine(0);
                        ql.SendToClient(client);
                        client.SendPacket(new NPCDialogPacket(_gameID));
                    }
                }
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

        public void NextDialog(Connection client)
        {
            Quest q = Program.Server.GetQuest(client.CurrentQuestID);
            byte step = client.Character.GetQuestStep(client.CurrentQuestID);
            byte line = client.CurrentQuestLine;

            // Next line
            line++;
            
            QuestStep qs = q.GetStep(step);
            if (line < qs.LineCount)
            {
                QuestLine ql = qs.GetLine(line);
                client.SetCurrentQuest(_id, q.QuestID, line);
                bool lastLine = (line >= (qs.LineCount - 1));
                ql.SendToClient(client, !lastLine);

                if( lastLine )
                    client.QuestDialogFinished(_id);
            }
            else
                client.SetCurrentQuest(_id, q.QuestID, 0);
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
