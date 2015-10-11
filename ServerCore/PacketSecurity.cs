using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuggleServerCore
{
    class PacketSecurity
    {
        ulong DecCounter = 0;
        ulong EncCounter = 0;
        bool m_FromServer;
        RockCrypto[] ScMethods = new RockCrypto[2];
        RockCrypto[] CsMethods = new RockCrypto[4];

        public PacketSecurity(bool FromServer)
        {
            byte[] Key = null;
            m_FromServer = FromServer;

            #region Server
            Key = new byte[]
            {
                0xE8, 0x17, 0x33, 0x00, 0x00, 0x8D, 0x45, 0xC8, 0x8B, 0xCF, 0x50, 0xE8, 0xF9, 0x23, 0x00, 0xFF
            };
            ScMethods[0] = new RockCrypto(null, Key, null, null);

            Key = new byte[]
            {
                0x66, 0x8D, 0x45, 0xEC, 0xF6, 0x50, 0x8B, 0xCF, 0x89, 0x35, 0x8C, 0x4A, 0x00, 0xE8, 0x2C, 0x86
            };
            ScMethods[1] = new RockCrypto(null, Key, null, null);
            #endregion

            #region Client
            Key = new byte[]
            {
                0x7D, 0x44, 0x01, 0x00, 0x83, 0xEC, 0x24, 0x83, 0x25, 0xB8, 0x8C, 0x4A, 0x0D, 0x56, 0x8B, 0x75
            };
            CsMethods[0] = new RockCrypto(null, Key, null, null);

            Key = new byte[]
            {
                0x1C, 0x8D, 0x1C, 0x57, 0x50, 0xCE, 0xE8, 0x6F, 0x85, 0xFE, 0xFF, 0x8B
            };
            CsMethods[1] = new RockCrypto(Key, null, null, null);
            Key = new byte[]
            {
                0x76, 0x0C, 0x50, 0x45, 0x14, 0x83, 0x65, 0xFC, 0x56, 0x50, 0x7D, 0xD1, 0x74, 0x03, 0xB8, 0x43
            };
            CsMethods[2] = new RockCrypto(null, Key, null, null);
            Key = new byte[]
            {
                0x8B, 0x47, 0xDD, 0x6A, 0xE8, 0x14, 0x83, 0xC4, 0xBC, 0xF3, 0x7F, 0x75
            };
            CsMethods[3] = new RockCrypto(Key, null, null, null);
            #endregion
        }

        public byte[] DecryptPacket(byte[] Encrypted, int offset, int length)
        {
            int Tries = 0;
            byte[] Result = null;
            DecCounter++;
            if (m_FromServer)
            {
            Retry:
                Result = ScMethods[DecCounter % 2].DecryptBlock(Encrypted, offset, length);
                if (Tries >= ScMethods.Length)
                {
                    DecCounter -= (ulong)ScMethods.Length;
                    return Result;
                }
                if (Result[0] + BitConverter.ToUInt16(Result, 1) != Result.Length)
                {
                    Tries++;
                    DecCounter++;
                    goto Retry;
                }
            }
            else
            {
            Retry:
                Result = CsMethods[DecCounter % 4].DecryptBlock(Encrypted, offset, length);
                if (Tries >= CsMethods.Length)
                {
                    DecCounter -= (ulong)CsMethods.Length;
                    return Result;
                }
                if (Result[0] + BitConverter.ToUInt16(Result, 1) != Result.Length)
                {
                    Tries++;
                    DecCounter++;
                    goto Retry;
                }
            }
            return Result;
        }

        public byte[] EncryptPacket(byte[] Packet)
        {
            EncCounter++;
            if (m_FromServer)
            {
                return ScMethods[EncCounter % 2].EncryptBlock(Packet);
            }
            else
            {
                return CsMethods[EncCounter % 4].EncryptBlock(Packet);
            }
        }

        public void EncryptInPlace(byte[] Packet)
        {
            EncCounter++;
            if (m_FromServer)
            {
                ScMethods[EncCounter % 2].EncryptBlockInPlace(Packet);
            }
            else
            {
                CsMethods[EncCounter % 4].EncryptBlockInPlace(Packet);
            }
        }
    }
}
