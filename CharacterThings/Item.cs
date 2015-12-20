using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoServer2
{
    public class Item
    {
        public enum Type
        {
            Equipped,
            General,
            Stackable,
            Quest,
            Riding
        }

        uint _id;
        ushort _model;
        byte _slot;
        ushort _durability;
        ushort _remainingTime;
        Type _type;
        uint _templateID;
        ItemTemplate _template;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(_id);              // 0xF  - DWORD - Item ID
            bw.Write(_model);           // 0x13 - WORD - Model ID
            bw.Write((byte)0);          // 0x15 - BYTE - Unused by client
            bw.Write(_slot);            // 0x16 - BYTE - Slot
            bw.Write((uint)0);          // 0x17 - DWORD - Unused by client
            bw.Write(_durability);      // 0x1B - WORD - Durability/Quantity
            bw.Write(_remainingTime);   // 0x1D - WORD - Not used by client?
        }

        public string WriteDBString(uint itemTemplate, int characterID)
        {
            string sql = string.Format("INSERT INTO item_instances SET template_id={0},durability={1},character_id={2},inventory_type={3},slot={4}; SELECT LAST_INSERT_ID();", itemTemplate, _durability, characterID, (byte)_type, _slot);
            return sql;
        }

        public string UpdateDBString()
        {
            string sql = string.Format("UPDATE item_instances SET durability={0},inventory_type={1},slot={2} WHERE instance_id={3};", _durability, (byte)_type, _slot, _id);
            return sql;
        }

        public void AddQuantity(int quantity)
        {
            _durability = (ushort)(_durability + quantity);
        }

        public static Item ReadFromDB(object[] row)
        {
            // 0: instance_id int(10) unsigned
            // 1: template_id int(10) unsigned
            // 2: durability  tinyint(3) unsigned
            // 3: character_id    int(10) unsigned
            // 4: inventory_type  tinyint(3) unsigned
            // 5: slot    tinyint(3) unsigned


            Item i = new Item();
            i._id = (uint)row[0];
            i._templateID = (uint)row[1];
            i._durability = (byte)row[2];
            i._type = (Type)((byte)row[4]);
            i._slot = (byte)(row[5]);

            i._template = Program.Server.GetItemTemplate(i._templateID);
            i._model = i._template.Model;

            return i;
        }

        public static Item Instantiate(ItemTemplate it)
        {
            // Setup the item
            Item item = new Item();
            item._id = 0;
            item._model = it.Model;
            item._durability = it.GenerateDurability();
            item._remainingTime = 0;
            item._type = (Type)it.Type;
            item._slot = 0xFF;
            item._templateID = it.ID;
            item._template = it;

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

        public ushort Quantity
        {
            get { return _durability; }
        }

        public int StackSpace
        {
            get { return _template.DQMax - _durability; }
        }

        public ushort RemainingTime
        {
            get { return _remainingTime; }
        }

        public uint TemplateID
        {
            get { return _templateID; }
        }
        #endregion
    }
}
