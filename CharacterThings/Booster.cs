using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoServer2
{
    public class Booster
    {
        ushort _id; // 11114, 11115, 11116
        uint _remainingTime;

        public Booster(ushort id, uint time)
        {
            _id = id;
            _remainingTime = time;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(_id);
            bw.Write(_remainingTime);
        }
    }
}
