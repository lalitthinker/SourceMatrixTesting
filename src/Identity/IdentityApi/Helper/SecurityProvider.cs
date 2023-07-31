using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace IdentityApi.Helper
{
    public class SecurityProvider
    {
        #region Fields

        //private readonly SecuritySettings _securitySettings;
        private const string EncryptionKey = "273ece6f97dd844d";

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="securitySettings">Security settings</param>
        //public EncryptionService(SecuritySettings securitySettings)
        //{
        //    this._securitySettings = securitySettings;
        //}

        #endregion

        #region Utilities

        private static Task<byte[]> EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, transform: new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                byte[] toEncrypt = Encoding.Unicode.GetBytes(data);
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }

            return Task.FromResult(ms.ToArray());
        }

        private static Task<string> DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using MemoryStream ms = new(data);
            using CryptoStream cs = new(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using StreamReader sr = new(cs, Encoding.Unicode);
            return Task.FromResult(sr.ReadToEnd());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public virtual string CreateSaltKey(int size)
        {
            //generate a cryptographic random number
            using RNGCryptoServiceProvider provider = new();
            byte[] buff = new byte[size];
            provider.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptUsernameAsync(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return Task.FromResult(plainText).Result;

            Encoding encoding = Encoding.Unicode;
            byte[] stringBytes = encoding.GetBytes(plainText);
            StringBuilder sbBytes = new(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return Task.FromResult(sbBytes.ToString()).Result;
        }

        public static string EncryptTextAsync(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            //var isBase64Check = Regex.Match(plainText, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$");

            //if (isBase64Check.Success == true)
            //    return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = EncryptionKey;

            using TripleDESCryptoServiceProvider provider = new()
            {
                Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8))
            };
            //provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
            //provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV).Result;
            string encryptedData = Convert.ToBase64String(encryptedBinary);
            return encryptedData;
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptUsernameAsync(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return Task.FromResult(cipherText).Result;

                Encoding encoding = Encoding.Unicode;
                int CharsLength = cipherText.Length;
                byte[] bytesarray = new byte[CharsLength / 2];
                for (int i = 0; i < CharsLength; i += 2)
                {
                    bytesarray[i / 2] = Convert.ToByte(cipherText.Substring(i, 2), 16);
                }
                return Task.FromResult(encoding.GetString(bytesarray)).Result;
            }
            catch
            {
                return Task.FromResult(cipherText).Result;
            }

        }

        public static string DecryptTextAsync(string cipherText, string encryptionPrivateKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return cipherText;

                //var isBase64Check = Regex.Match(cipherText, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$");

                //if (isBase64Check.Success == false)
                //    return cipherText;

                if (string.IsNullOrEmpty(encryptionPrivateKey))
                    encryptionPrivateKey = EncryptionKey;

                using TripleDESCryptoServiceProvider provider = new()
                {
                    Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16)),
                    IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8))
                };

                byte[] buffer = Convert.FromBase64String(cipherText);
                string decryptedData = DecryptTextFromMemory(buffer, provider.Key, provider.IV).Result;
                return decryptedData;
            }
            catch
            {
                return cipherText;
            }

        }

        public static string EncryptNewAlgoAsync(string Source)
        {
            try
            {
                string strRet = null;
                string strSub = null;
                ArrayList arrOffsets = new();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;

                arrOffsets.Insert(0, 73);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 58);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 75);

                strRet = "";

                for (intCounter = 0; intCounter <= Source.Length - 1;
                intCounter++)
                {
                    strSub = Source.Substring(intCounter, 1);
                    intVal =
                    (int)System.Text.Encoding.ASCII.GetBytes(strSub)[0];
                    intMod = intCounter % arrOffsets.Count;
                    intNewVal = intVal +
                    Convert.ToInt32(arrOffsets[intMod]);
                    intNewVal %= 256;
                    strRet += intNewVal.ToString("X2");
                }
                return Task.FromResult(strRet).Result;

            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message).Result;
            }
        }

        public  static string DecryptNewAlgoAsync(string Source)
        {
            try
            {
                ArrayList arrOffsets = new();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;
                string strOut = null;
                string strSub = null;
                string strSub1 = null;
                string strDecimal = null;

                arrOffsets.Insert(0, 73);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 58);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 75);

                strOut = "";
                for (intCounter = 0; intCounter <= Source.Length - 1;
                intCounter += 2)
                {
                    strSub = Source.Substring(intCounter, 1);
                    strSub1 = Source.Substring((intCounter + 1), 1);
                    intVal = int.Parse(strSub,
                    System.Globalization.NumberStyles.HexNumber) * 16 + int.Parse(strSub1,
                    System.Globalization.NumberStyles.HexNumber);
                    intMod = (intCounter / 2) % arrOffsets.Count;
                    intNewVal = intVal -
                    Convert.ToInt32(arrOffsets[intMod]) + 256;
                    intNewVal %= 256;
                    strDecimal = ((char)intNewVal).ToString();
                    strOut += strDecimal;
                }
                return Task.FromResult(strOut).Result;
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message).Result;
            }
        }

        public static string EncryptTextAsync1Async(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            //var isBase64Check = Regex.Match(plainText, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$");

            //if (isBase64Check.Success == true)
            //    return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = "273ece6f97dd844d";

            using TripleDESCryptoServiceProvider provider = new()
            {
                Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8))
            };
            //provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
            //provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV).Result;
            string encryptedData = Convert.ToBase64String(encryptedBinary);
            return encryptedData;
        }
        #endregion
    }
}


