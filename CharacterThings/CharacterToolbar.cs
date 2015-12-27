using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuggleServerCore;

namespace DecoServer2
{
    public class CharacterToolbar
    {
        uint[] _slots;

        public CharacterToolbar()
        {
            _slots =  new uint[30];
            for( int i = 0; i < _slots.Length; i++ )
                _slots[i] = 0;
        }

        public void ReadFromDB(object[] row)
        {
            // 0: character_id    int(11)
            // 1: slot_0  int(10) unsigned
            // 2: slot_1  int(10) unsigned
            // 3: slot_2  int(10) unsigned
            // 4: slot_3  int(10) unsigned
            // 5: slot_4  int(10) unsigned
            // 6: slot_5  int(10) unsigned
            // 7: slot_6  int(10) unsigned
            // 8: slot_7  int(10) unsigned
            // 9: slot_8  int(10) unsigned
            // 10: slot_9  int(10) unsigned
            // 11: slot_10 int(10) unsigned
            // 12: slot_11 int(10) unsigned
            // 13: slot_12 int(10) unsigned
            // 14: slot_13 int(10) unsigned
            // 15: slot_14 int(10) unsigned
            // 16: slot_15 int(10) unsigned
            // 17: slot_16 int(10) unsigned
            // 18: slot_17 int(10) unsigned
            // 19: slot_18 int(10) unsigned
            // 20: slot_19 int(10) unsigned
            // 21: slot_20 int(10) unsigned
            // 22: slot_21 int(10) unsigned
            // 23: slot_22 int(10) unsigned
            // 24: slot_23 int(10) unsigned
            // 25: slot_24 int(10) unsigned
            // 26: slot_25 int(10) unsigned
            // 27: slot_26 int(10) unsigned
            // 28: slot_27 int(10) unsigned
            // 29: slot_28 int(10) unsigned
            // 30: slot_29 int(10) unsigned

            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = (uint)row[i + 1];
            }
        }

        public void SendToClient(Connection client, bool clear = true)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if( _slots[i] != 0 )
                    client.SendPacket(new SetToolbarLink(_slots[i] & 0x7FFFFFFF, ((_slots[i] & 0x80000000) == 0) ? ToolbarItemSetRequest.TargetType.Item : ToolbarItemSetRequest.TargetType.Skill, i));
                else if (clear)
                    client.SendPacket(new ClearToolbarLink((byte)i));
            }
        }

        public string SetItem(Connection client, ToolbarItemSetRequest tbr)
        {
            // Change the data in memory
            _slots[tbr.Slot] = tbr.TargetID | (tbr.Type == ToolbarItemSetRequest.TargetType.Skill ? 0x80000000 : 0);

            // Tell the client we accept
            client.SendPacket(new SetToolbarLink(tbr.TargetID, tbr.Type, tbr.Slot));

            // Save it in the database
            string sql = string.Format("UPDATE characters_toolbar SET slot_{0}={1} WHERE character_id={2};", tbr.Slot, _slots[tbr.Slot], client.Character.ID);
            
            return sql;
        }

        public string ClearItem(Connection client, byte slot)
        {
            // Change the data in memory
            _slots[slot] = 0;

            // Tell the client
            client.SendPacket(new ClearToolbarLink(slot));

            // Save in the database
            string sql = string.Format("UPDATE characters_toolbar SET slot_{0}=0 WHERE character_id={1};", slot, client.Character.ID);
            return sql;
        }
    }
}
