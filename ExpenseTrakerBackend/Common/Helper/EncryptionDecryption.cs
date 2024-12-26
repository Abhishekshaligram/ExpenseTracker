using System.Security.Cryptography;
using System.Text;

namespace PracticeCrud.Common.Helper
{
    public class EncryptionDecryption
    {
        #region Variable Declaration

        /// <summary>
        /// key String
        /// </summary> 
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;
        private const int _iterations = 100000;
        private const string keyString = "EA34FF3E-JU84-1974-AW70-BB81D9564426";
        private const int _keySize = 32; // 256 bits
        private const int _saltSize = 16; // 128 bits
        private const char segmentDelimiter = ':';
        #endregion

        #region SuperariLife - Encrypt/Decrypt

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Decrypt</param>
        /// <param name="strData">Message to Decrypt</param>
        /// <returns>Decrypted string</returns>
        private static string Decrypt(string strKey, string strData)
        {
            string clearText = "";
            byte[] cipherBytes = Convert.FromBase64String(strData);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strKey, new byte[] { 0x56, 0x61, 0x69, 0x62, 0x48, 0x61, 0x76, 0x50, 0x61, 0x72, 0x65, 0x6b, 0x68 }, _iterations, _algorithm);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return clearText.Replace("&quot;", @"""");
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Encrypt</param>
        /// <param name="strData">Message to Encrypt</param>
        /// <returns>encrypted string</returns>
        private static string Encrypt(string strKey, string strData)
        {
            string cipherText = "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(strData);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strKey, new byte[] { 0x56, 0x61, 0x69, 0x62, 0x48, 0x61, 0x76, 0x50, 0x61, 0x72, 0x65, 0x6b, 0x68 }, _iterations, _algorithm);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Mode = CipherMode.CBC;
                encryptor.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return cipherText;
        }

        /// <summary>
        /// Get Encrypted Value of Passed value
        /// </summary>
        /// <param name="value">value to Encrypted</param>
        /// <returns>encrypted string</returns>
        public static string GetEncrypt(string value)
        {
            return Encrypt(keyString, value);
        }


        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value">value to Decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string GetDecrypt(string value)
        {
            return Decrypt(keyString, value);
        }

        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                _iterations,
                _algorithm,
                _keySize
            );
            return string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                _iterations,
                _algorithm
            );
        }
        public static bool Verify(string input, string hashString, string saltString)
        {
            byte[] hash = Convert.FromHexString(hashString);
            byte[] salt = Convert.FromHexString(saltString);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                _iterations,
                _algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }


        /// <summary>
        /// Get Encrypted Value of Passed value
        /// </summary>
        /// <param name="value">value to Encrypted</param>
        /// <returns>encrypted string</returns>
        public static string GetEncryptNew(string value)
        {
            return EncryptNew(keyString, value);
        }


        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value">value to Decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string GetDecryptNew(string value)
        {
            return DecryptNew(keyString, value);
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Encrypt</param>
        /// <param name="strData">Message to Encrypt</param>
        /// <returns>encrypted string</returns>
        private static string EncryptNew(string strKey, string strData)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            // Step 3. Setup the encoder

            // Step 4. Convert the input string to a byte[]
            byte[] dataToEncrypt = utf8.GetBytes(strData);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(results);
        }

        /// <summary>
        /// decrypt value
        /// </summary>
        /// <param name="strKey">Passphrase for Decrypt</param>
        /// <param name="strData">Message to Decrypt</param>
        /// <returns>Decrypted string</returns>
        private static string DecryptNew(string strKey, string strData)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            // Step 3. Setup the decoder

            strData = strData.Replace(" ", "+"); // Replace space with plus sign in encrypted value if any.- kalpesh joshi [09/05/2013]

            // Step 4. Convert the input string to a byte[]
            byte[] dataToDecrypt = Convert.FromBase64String(strData);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }
        #endregion
    }
}
