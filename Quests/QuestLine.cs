using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoServer2.Quests
{
    public class QuestLine
    {
        uint _id;
        uint _quest;
        byte _step;
        byte _line;
        ushort _icon;
        ushort _staticText;
        string _text;

        public static QuestLine ReadFromDB(object[] row)
        {
            // 0: quest_line_id     int(10) unsigned
            // 1: quest_id          int(10) unsigned
            // 2: step              int(10) unsigned
            // 3: line              int(10) unsigned
            // 4: icon              smallint(5) unsigned
            // 5: static_text       smallint(5) unsigned
            // 6: text              varchar(255)

            QuestLine ql = new QuestLine();

            ql._id = (uint)row[0];
            ql._quest = (uint)row[1];
            ql._step = (byte)row[2];
            ql._line = (byte)row[3];
            ql._icon = (ushort)row[4];
            ql._staticText = (ushort)row[5];
            if( row[6].GetType() != typeof(System.DBNull) )
                ql._text = (string)row[6];

            return ql;
        }

        #region Accessors
        public uint QuestID
        {
            get { return _quest; }
        }

        public byte StepNumber
        {
            get { return _step; }
        }

        public byte LineNumber
        {
            get { return _line; }        
        }

        public ushort Icon
        {
            get { return _icon; }
        }

        public ushort StaticText
        {
            get { return _staticText; }
        }

        public string Text
        {
            get { return _text; }
        }
        #endregion
    }
}
