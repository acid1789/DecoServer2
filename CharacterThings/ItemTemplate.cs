using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2
{
    public class ItemTemplate
    {
        uint _id;
        ushort _model;
        byte _type;
        byte _genDQMin;
        byte _genDQMax;
        byte _dqMax;

        public Item Instantiate()
        {
            return Item.Instantiate(this);
        }

        public byte GenerateDurability()
        {
            Random rand = new Random();
            return (byte)rand.Next(_genDQMin, _genDQMax);
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

        public byte DQMax
        {
            get { return _dqMax; }
        }

        public static ItemTemplate ReadFromDB(object[] row)
        {
            // 0: item_template_id    int(10) unsigned
            // 1: model   smallint(5) unsigned
            // 2: type    tinyint(3) unsigned
            // 3: gen_dq_min  tinyint(3) unsigned
            // 4: gen_dq_max  tinyint(3) unsigned
            // 5: dq_max  tinyint(3) unsigned

            ItemTemplate it = new ItemTemplate();
            it._id = (uint)row[0];
            it._model = (ushort)row[1];
            it._type = (byte)row[2];
            it._genDQMin = (byte)row[3];
            it._genDQMax = (byte)row[4];
            it._dqMax = (byte)row[5];

            return it;
        }
    }
}
