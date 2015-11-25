using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JuggleServerCore;
using DecoServer2.CharacterThings;
using DecoServer2.Quests;

namespace DecoServer2
{
    public class PlayMap
    {
        Dictionary<int, Connection> _players;
        Dictionary<uint, NPC> _npcs;

        ushort _mapID;
        uint _worldIDs;

        public PlayMap(ushort mapID)
        {
            _worldIDs = 0;
            _npcs = new Dictionary<uint, NPC>();
            _players = new Dictionary<int, Connection>();
            _mapID = mapID;
        }

        public void AddNPC(NPC npc)
        {
            _npcs[npc.ID] = npc;
        }

        public void AddPlayer(Connection client, CharacterInfo ci)
        {     
            if (!_players.ContainsKey(ci.ID))
            {
                ci.WorldID = _worldIDs++;

                // Send NPC info to player
                foreach (NPC npc in _npcs.Values)
                {
                    client.SendPacket(new NPCInfoPacket(npc));
                }

                // Send player character info to all other players
                // TODO
            }

            _players[ci.ID] = client;            
        }

        public void RemovePlayer(CharacterInfo ci)
        {
            if (_players.ContainsKey(ci.ID))
            {
                // Tell all other players that this one is gone
                // TODO

                // Remove the player
                _players.Remove(ci.ID);
            }
        }

        public void ProcessMoveRequest(Connection client, CharacterPositionClass mtp)
        {
            // Check to see if we hit any npcs
            foreach (NPC npc in _npcs.Values)
            {
                if (npc.CellIndex == mtp.CellIndex)
                {
                    // Hit this npc - Check distance
                    Vector target = Utils.DecodeCellIndex(_mapID, mtp.CellIndex);
                    Vector start = Utils.DecodeCellIndex(_mapID, client.Character.CellIndex);
                    Vector toTarget = target - start;
                    double distSquared = toTarget.LengthSquared;
                    if (distSquared > 13)
                    {
                        // Out of range, just move there
                        client.SendPacket(new PlayerMovePacket(mtp.CellIndex, client.Character.MoveSpeed));
                    }
                    else
                    {
                        // Talk to NPC
                        npc.DoDialog(client);
                    }

                    // no matter what, we are done here
                    return;
                }
            }

            // Check to see if we hit monsters

            // Check to see if we hit other players

            // Just move
            client.SendPacket(new PlayerMovePacket(mtp.CellIndex, client.Character.MoveSpeed));
        }

        public void UpdatePlayerPosition(Connection client, CharacterPositionClass cpc)
        {
            client.Character.CellIndex = cpc.CellIndex;            

            // Inform other clients            
            ObserveMovementPacket omp = new ObserveMovementPacket(client.Character.WorldID, cpc.CellIndex, client.Character.MoveSpeed);
            foreach (Connection c in _players.Values)
            {
                if (c.AccountID != client.AccountID)
                {
                    // Send move update
                    c.SendPacket(omp);
                }
            }
        }

        public void SetQuestGiver(Quest q, uint giverID)
        {
            _npcs[giverID].AddQuest(q);
        }

        public void NextDialog(Connection client)
        {
            _npcs[client.CurrentQuestNPC].NextDialog(client);
        }


        #region Accessors
        public Connection[] Players
        {
            get { return _players.Values.ToArray(); }
        }
        #endregion
    }
}
