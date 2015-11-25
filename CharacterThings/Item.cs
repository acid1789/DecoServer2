using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoServer2.CharacterThings
{
    public class Item
    {
        public enum Type
        {
            Equipped,
            General,
            Item,
            Quest,
            Riding
        }

        uint _id;
        ushort _model;
        byte _slot;
        ushort _durability;
        ushort _remainingTime;
        Type _type;
        uint _template;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(_id);
            bw.Write(_model);
            bw.Write((byte)1);      // Unknown byte
            bw.Write(_slot);
            bw.Write((uint)1);      // Unknown dword
            bw.Write(_durability);
            bw.Write(_remainingTime);
        }

        public string WriteDBString(uint itemTemplate, int characterID)
        {
            string sql = string.Format("INSERT INTO item_instances SET template_id={0},durability={1},remaining_time={2},character_id={3},inventory_type={4},slot={5}; SELECT LAST_INSERT_ID();", itemTemplate, _durability, _remainingTime, characterID, (byte)_type, _slot);
            return sql;
        }

        public string UpdateDBString()
        {
            string sql = string.Format("UPDATE item_instances SET durability={0},remaining_time={1},inventory_type={2},slot={3} WHERE instance_id={4};", _durability, _remainingTime, (byte)_type, _slot, _id);
            return sql;
        }

        public static Item ReadFromDB(object[] row)
        {
            // 0: instance_id       int(10) unsigned
            // 1: template_id       int(10) unsigned
            // 2: durability        smallint(5) unsigned
            // 3: remaining_time    smallint(5) unsigned
            // 4: character_id      int(10) unsigned
            // 5: inventory_type    int(10) unsigned
            // 6: slot              tinyint
            

            Item i = new Item();
            i._id = (uint)row[0];
            i._template = (uint)row[1];
            i._durability = (ushort)row[2];
            i._remainingTime = (ushort)row[3];
            i._type = (Type)((uint)row[5]);
            i._slot = (byte)(row[6]);

            ItemTemplate t = Program.Server.GetItemTemplate(i._template);
            i._model = t.Model;

            return i;
        }

        public static Item Instantiate(ItemTemplate it)
        {
            // Setup the item
            Item item = new Item();
            item._id = 0;
            item._model = it.Model;
            item._durability = it.GenerateDurability();
            item._remainingTime = it.GenerateDuration();
            item._type = (Type)it.Type;
            item._slot = 0xFF;
            item._template = it.ID;
            
            return item;
        }


        #region Accessors
        public Type ItemType
        {
            get { return _type; }
            set { _type = value; }
        }

        public byte Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        public uint ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public ushort Model
        {
            get { return _model; }
        }

        public ushort Durability
        {
            get { return _durability; }
        }

        public ushort RemainingTime
        {
            get { return _remainingTime; }
        }

        public uint TemplateID
        {
            get { return _template; }
        }
        #endregion
    }
}
