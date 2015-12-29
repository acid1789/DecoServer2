using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JuggleServerCore;

namespace DecoServer2
{
    public class CharacterSelectInfo
    {
        bool _valid;
        int _id;
        string _name;
        int _modelInfo;
        sbyte _job;
        sbyte _level;

        int _shirt;
        int _pants;
        int _rightHand;
        int _leftHand;
        int _hat;
        int _suit;
        int _gloves;
        int _boots;
        int _neck1;
        int _neck2;

        public CharacterSelectInfo()
        {
            _valid = false;
            _id = -1;
        }

        public CharacterSelectInfo(object[] dbRow)
        {
            if (dbRow.Length == 16)
            {
                // 0: character_id
                // 1: name
                // 2: model_info
                // 3: job
                // 4: level
                // 5: shirt
                // 6: pants
                // 7: right_hand
                // 8: left_hand
                // 9: hat
                // 10: suit
                // 11: gloves
                // 12: boots
                // 13: neck1
                // 14: neck2
                // 15: account_id

                _valid = true;
                _id = (int)dbRow[0];
                _name = (string)dbRow[1];
                _modelInfo = (int)dbRow[2];
                _job = (sbyte)dbRow[3];
                _level = (sbyte)dbRow[4];
                _shirt = (int)dbRow[5];
                _pants = (int)dbRow[6];
                _rightHand = (int)dbRow[7];
                _leftHand = (int)dbRow[8];
                _hat = (int)dbRow[9];
                _suit = (int)dbRow[10];
                _gloves = (int)dbRow[11];
                _boots = (int)dbRow[12];
                _neck1 = (int)dbRow[13];
                _neck2 = (int)dbRow[14];
            }
        }

        public void Write(BinaryWriter bw)
        {
            if (_valid)
            {
                bw.Write((uint)1);

                Utils.WriteByteString(bw, _name, 17);
                bw.Write(_modelInfo);
                bw.Write(_job);

                Utils.WriteZeros(bw, 5);
                bw.Write(_level);
                Utils.WriteZeros(bw, 22);
                bw.Write(_shirt);
                bw.Write(_pants);
                bw.Write(_leftHand);
                bw.Write(_rightHand);
                bw.Write(_hat);
                bw.Write(_suit);
                bw.Write(_gloves);
                bw.Write(_boots);
                bw.Write(_neck1);
                bw.Write(_neck2);

                Utils.WriteZeros(bw, 38);
            }
            else
            {
                bw.Write((int)10);
                Utils.WriteZeros(bw, 128, true);
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public int ID
        {
            get { return _id; }
        }

        public bool IsValid
        {
            get { return _valid; }
        }

        public int ModelInfo
        {
            get { return _modelInfo; }
        }

        public byte Job
        {
            get { return (byte)_job; }
        }

        public byte Level
        {
            get { return (byte)_level; }
            set { _level = (sbyte)value; }
        }

        public bool Millena
        {
            get { return Utils.NationFromModelInfo(_modelInfo); }
        }

        public bool Male
        {
            get { return Utils.GenderFromModelInfo(_modelInfo); }
        }

        public string UpdateString
        {
            get
            {
                return string.Format("UPDATE characters SET model_info={0},job={1},level={2},shirt={3},pants={4},right_hand={5},left_hand={6},hat={7},suit={8},gloves={9},boots={10},neck1={11},neck2={12} WHERE character_id={13};", 
                                                            _modelInfo, _job, _level, _shirt, _pants, _rightHand, _leftHand, _hat, _suit, _gloves, _boots, _neck1, _neck2, _id);
            }
        }
    }
}
