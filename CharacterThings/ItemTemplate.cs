using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2.CharacterThings
{
    public class ItemTemplate
    {
        uint _id;
        ushort _model;
        uint _type;
        ushort _durabilityMin;
        ushort _durabilityMax;
        ushort _durationMin;
        ushort _durationMax;

        public Item Instantiate()
        {
            return Item.Instantiate(this);
        }

        public ushort GenerateDurability()
        {
            Random rand = new Random();
            return (ushort)rand.Next((int)_durabilityMin, (int)_durabilityMax);
        }

        public ushort GenerateDuration()
        {
            Random rand = new Random();
            return (ushort)rand.Next((int)_durationMin, (int)_durationMax);
        }

        public uint ID
        {
            get { return _id; }
        }

        public ushort Model
        {
            get { return _model; }
        }

        public uint Type
        {
            get { return _type; }
        }

        public static ItemTemplate ReadFromDB(object[] row)
        {
            // 0: item_template_id    int(10) unsigned
            // 1: model   int(10) unsigned
            // 2: type    int(10) unsigned
            // 3: durability_min  int(10) unsigned
            // 4: durability_max  int(10) unsigned
            // 5: duration_min    int(10) unsigned
            // 6: duration_max    int(10) unsigned

            ItemTemplate it = new ItemTemplate();
            it._id = (uint)row[0];
            it._model = (ushort)row[1];
            it._type = (uint)row[2];
            it._durabilityMin = (ushort)row[3];
            it._durabilityMax = (ushort)row[4];
            it._durationMin = (ushort)row[5];
            it._durationMax = (ushort)row[6];

            return it;
        }
    }
}
