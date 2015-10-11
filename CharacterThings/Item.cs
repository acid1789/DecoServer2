using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoServer2.CharacterThings
{
    public class ItemTemplate
    {
        public uint Icon;
        public ushort Model;
    }

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

        static ItemTemplate[] s_ItemTemplates;

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

            ItemTemplate t = s_ItemTemplates[template];
            i._icon = t.Icon;
            i._model = t.Model;

            return i;
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
        #endregion
    }
}
