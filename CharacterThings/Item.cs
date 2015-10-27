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
            Clothing,
            General,
            Item,
            Quest,
            Riding
        }

        uint _id;
        uint _icon;
        ushort _model;
        byte _slot;
        ushort _durability;
        ushort _remainingTime;
        Type _type;

        public void Write(BinaryWriter bw)
        {
            bw.Write(_icon);
            bw.Write(_model);
            bw.Write((byte)1);      // Unknown byte
            bw.Write(_slot);
            bw.Write((uint)2);      // Unknown dword
            bw.Write(_durability);
            bw.Write(_remainingTime);
        }

        public string WriteDBString(uint itemTemplate, int characterID)
        {
            string sql = string.Format("INSERT INTO item_instances SET template_id={0},durability={1},remaining_time={2},character_id={3},inventory_type{4};", itemTemplate, _durability, _remainingTime, characterID, _type);
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

            Item i = new Item();
            i._id = (uint)row[0];
            uint template = (uint)row[1];
            i._durability = (ushort)row[2];
            i._remainingTime = (ushort)row[3];
            i._type = (Type)((uint)row[5]);

            ItemTemplate t = Program.Server.GetItemTemplate(template);
            i._icon = t.ID;
            i._model = t.Model;

            return i;
        }

        public static Item Instantiate(ItemTemplate it)
        {
            // Setup the item
            Item item = new Item();
            item._icon = it.ID;
            item._model = it.Model;
            item._durability = it.GenerateDurability();
            item._remainingTime = it.GenerateDuration();
            item._type = (Type)it.Type;
            
            return item;
        }


        #region Accessors
        public Type ItemType
        {
            get { return _type; }
        }

        public byte Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        public uint Icon
        {
            get { return _icon; }
        }
        #endregion
    }
}
