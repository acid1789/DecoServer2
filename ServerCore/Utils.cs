using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace JuggleServerCore
{
    public static class Utils
    {
        public static string ReadByteString(BinaryReader br, int byteCount)
        {
            if( byteCount <= 0 )
                return null;

            byte[] data = br.ReadBytes(byteCount);
            string str = Encoding.ASCII.GetString(data);
            int idx = str.IndexOf('\0');
            if( idx >= 0 )
                str = str.Substring(0, idx);
            return str;
        }

        public static void WriteByteString(BinaryWriter bw, string str, int byteCount)
        {
            if (str == null)
                WriteZeros(bw, byteCount);
            else
            {
                byte[] encoded = Encoding.ASCII.GetBytes(str);
                if (encoded.Length >= byteCount)
                {
                    bw.Write(encoded, 0, byteCount - 1);
                    bw.Write((byte)0);
                }
                else
                {
                    bw.Write(encoded);
                    bw.Write((byte)0);
                    WriteZeros(bw, byteCount - (encoded.Length + 1));
                }
            }
        }

        public static void WriteZeros(BinaryWriter bw, int zeroCount, bool debugOverride = true)
        {
            bool debugFillData = false;
#if DEBUG
            debugFillData = !debugOverride;
#endif

            if (zeroCount > 0)
            {
                if (debugFillData)
                {
                    int dwords = zeroCount >> 2;
                    int remaining = zeroCount - (dwords << 2);
                    for (int i = 0; i < dwords; i++)
                        bw.Write((uint)0x0DF0ADBA);
                    for (int i = 0; i < remaining; i++)
                        bw.Write((byte)0xEC);
                }
                else
                {
                    byte[] zeros = new byte[zeroCount];
                    bw.Write(zeros);
                }
            }
        }

        public static uint PackModelInfo(bool male, bool millena, int face, int hair)
        {
            int packed = ((face & 0x7FFF) << 2) | ((hair & 0x7FFF) << 17);        
            if( millena )
                packed |= 1;
            if( male )
                packed |= 2;

            return (uint)packed;
        }

        public static bool NationFromModelInfo(int modelInfo)
        {
            int nation = modelInfo & 0x1;
            return (nation != 0);
        }

        public static bool GenderFromModelInfo(int modelInfo)
        {
            int gender = modelInfo & 0x2;
            return (gender != 0);
        }

        public static uint GetMapSize(ushort map)
        {
            uint mapSize = 512;
            switch (map)
            {
                case 33:
                case 39:
                case 40:
                    mapSize = 128;
                    break;
                case 5:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 34:
                case 35:
                case 36:
                case 44:
                case 53:
                    mapSize = 256;
                    break;
            }
            return mapSize;
        }

        public static uint EncodeCellIndex(ushort map, Vector v)
        {
            return EncodeCellIndex(map, (uint)v.X, (uint)v.Y);
        }

        public static uint EncodeCellIndex(ushort map, uint x, uint y)
        {
            uint mapSize = GetMapSize(map);
            uint cell = (y * mapSize) + x;
            return cell;
        }

        public static Vector DecodeCellIndex(ushort map, uint cellIndex)
        {
            uint mapSize = GetMapSize(map);
            uint y = cellIndex / mapSize;
            uint x = cellIndex - (y * mapSize);
            return new Vector(x, y);
        }

        public static string PrintBinaryData(byte[] data)
        {
            int lineSize = 16;
            int lines = data.Length / lineSize;

            string output = "\n";

            for (int i = 0; i < lines; i++)
            {
                int offset = i * lineSize;
                string line = offset.ToString("X5") + ":";

                for (int j = 0; j < lineSize; j++)
                {
                    if ((j % 4) == 0)
                        line += " ";
                    line += data[offset + j].ToString("X2");
                }

                line += "  ";

                for (int j = 0; j < lineSize; j++)
                {
                    if ((j % 4) == 0)
                        line += " ";
                    byte b = data[offset + j];
                    if (b >= '!' && b <= '~')
                        line += (char)b;
                    else
                        line += ".";
                }

                output += line + "\n";
            }

            // Write extra bytes
            int remaining = data.Length - (lines * lineSize);
            if (remaining > 0)
            {
                int offset = lines * lineSize;
                string line = offset.ToString("X5") + ":";

                for (int j = 0; j < lineSize; j++)
                {
                    if ((j % 4) == 0)
                        line += " ";
                    if (j < remaining)
                        line += data[offset + j].ToString("X2");
                    else
                        line += "  ";
                }

                line += "  ";

                for (int j = 0; j < remaining; j++)
                {
                    if ((j % 4) == 0)
                        line += " ";
                    byte b = data[offset + j];
                    if (b >= '!' && b <= '~')
                        line += (char)b;
                    else
                        line += ".";
                }

                output += line + "\n";
            }
            return output;
        }
    }
}
