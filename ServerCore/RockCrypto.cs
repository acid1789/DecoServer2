using System;
using System.Runtime.InteropServices;

namespace JuggleServerCore
{
    /// <summary>
    /// Rock Crypto Class
    /// </summary>
    class RockCrypto
    {
        

        private byte[] _Data = new byte[120];
        
        [DllImport("RockBaseML.dll", EntryPoint = "??0CRockCrypto@RockBase@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        static extern int RockCrypto_Init(byte[] RVFClass);
        [DllImport("RockBaseML.dll", EntryPoint = "?SetKey@CRockCrypto@RockBase@@QAEXPBD000@Z", CallingConvention = CallingConvention.ThisCall)]
        static extern int RockCrypto_SetKey(byte[] RVFClass, byte[] StrmKey, byte[] RcKey, byte[] DesKey, byte[] AesKey);
        [DllImport("RockBaseML.dll", EntryPoint = "?Decrypt@CRockCrypto@RockBase@@QAEXPAXH@Z", CallingConvention = CallingConvention.ThisCall)]
        static extern int RockCrypto_Decrypt(byte[] RVFClass, byte[] Data, int Len);
        [DllImport("RockBaseML.dll", EntryPoint = "?Encrypt@CRockCrypto@RockBase@@QAEXPAXH@Z", CallingConvention = CallingConvention.ThisCall)]
        static extern int RockCrypto_Encrypt(byte[] RVFClass, byte[] Data, int Len);

        public RockCrypto( )
        {
            RockCrypto_Init(_Data);
        }

        /// <summary>
        /// Init Class with a Key
        /// </summary>
        /// <param name="StrmKey">The Key of 1st Step (Strm Crypto)</param>
        /// <param name="RcKey">The Key of 2nd Step (Rc Crypto)</param>
        /// <param name="DesKey">The Key of 3rd Step (Des Crypto)</param>
        /// <param name="AesKey">The Key of 4th Step (Aes Crypto)</param>
        public RockCrypto(byte[] StrmKey, byte[] RcKey, byte[] DesKey, byte[] AesKey)
        {
            RockCrypto_Init(_Data);
            SetKey(StrmKey, RcKey, DesKey, AesKey);
        }

        public RockCrypto(RockCrypto Cls)
        {
            _Data = Cls._Data;
        }

        /// <summary>
        /// Sets The Default Key of Deco Data
        /// </summary>
        public void SetRVFKey()
        {
            byte[] MainKey = new byte[]
	        {
		        0x6A, 0x2C, 0x67, 0xE1, 0x42, 0x34, 0x93, 0xBC, 0x0A, 0x76, 0xC1, 0x0B
	        };

            RockCrypto_SetKey(_Data, MainKey, null, null, null);
        }

        /// <summary>
        /// Sets A Custom Key
        /// </summary>
        /// <param name="StrmKey">The Key of 1st Step (Strm Crypto)</param>
        /// <param name="RcKey">The Key of 2nd Step (Rc Crypto)</param>
        /// <param name="DesKey">The Key of 3rd Step (Des Crypto)</param>
        /// <param name="AesKey">The Key of 4th Step (Aes Crypto)</param>
        public void SetKey(byte[] StrmKey, byte[] RcKey, byte[] DesKey, byte[] AesKey)
        {
            if (StrmKey == null && RcKey == null && DesKey == null && AesKey == null)
                throw new Exception("You must set one key at least");
            RockCrypto_SetKey(_Data, StrmKey, RcKey, DesKey, AesKey);
        }

        /// <summary>
        /// Decrypts Block of bytes
        /// </summary>
        /// <param name="Data">The buffer of Cipher Bytes</param>
        /// <returns>The buffer of Decrypted Bytes</returns>
        public byte[] DecryptBlock(byte[] Data)
        {
            byte[] Result = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Result, 0, Result.Length);
            RockCrypto_Decrypt(_Data, Result, Result.Length);
            return Result;
        }

        /// <summary>
        /// Decrypts Block of bytes
        /// </summary>
        /// <param name="Data">The buffer of Cipher Bytes</param>
        /// <returns>The buffer of Decrypted Bytes</returns>
        public byte[] DecryptBlock(byte[] Data, int Offset, int Len)
        {
            byte[] Result = new byte[Len];
            Buffer.BlockCopy(Data, Offset, Result, 0, Len);
            RockCrypto_Decrypt(_Data, Result, Result.Length);
            return Result;
        }

        /// <summary>
        /// Decrypts Block of bytes
        /// </summary>
        /// <param name="Data">The buffer of Cipher Bytes</param>
        /// <returns>The buffer of Decrypted Bytes</returns>
        public byte[] DecryptBlock(byte[] Data, int Offset, int Len, int CryptLen)
        {
            byte[] Result = new byte[Len];
            Buffer.BlockCopy(Data, Offset, Result, 0, Len);
            RockCrypto_Decrypt(_Data, Result, CryptLen);
            return Result;
        }


        /// <summary>
        /// Encrypts Block of bytes
        /// </summary>
        /// <param name="Data">The buffer of Cipher Bytes</param>
        /// <returns>The buffer of Encrypted Bytes</returns>
        public byte[] EncryptBlock(byte[] Data)
        {
            byte[] Result = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Result, 0, Result.Length);
            RockCrypto_Encrypt(_Data, Result, Result.Length);
            return Result;
        }

        public void EncryptBlockInPlace(byte[] Data)
        {
            RockCrypto_Encrypt(_Data, Data, Data.Length);
        }
    }
}
