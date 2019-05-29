using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
namespace Epi.Web.Common.Security
{
    /// <summary>
    /// This class contains two static methods one to entrypt string and one to decrypt string
    /// </summary>
    public class Cryptography
    {
        /// <summary>
        /// Identifier for Web driver that is built into Epi Info
        /// </summary>


        public static string passPhrase = "80787d6053694493be171dd712e51c61";
        public static string saltValue = "476ba16073764022bc7f262c6d67ebef";
        public static string initVector = "0f8f*d5bd&cb4~9f";
        public static string passPhrase_config = ConfigurationManager.AppSettings["KeyForConnectionStringPassphrase"];
        public static string saltValue_config = ConfigurationManager.AppSettings["KeyForConnectionStringSalt"];
        public static string initVector_config = ConfigurationManager.AppSettings["KeyForConnectionStringVector"];

        private static string TokenPassPhrase = ConfigurationManager.AppSettings["TokenPassPhrase"]; 
        private static string TokenSaltValue = ConfigurationManager.AppSettings["TokenSaltValue"]; 
        private static string TokenInitVector = ConfigurationManager.AppSettings["TokenInitVector"]; 

        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="plainText">The plaintext to encrypt</param>
        /// <returns>The ciphertext</returns>
        public static string Encrypt(string plainText)
        {
            byte[] initVectorBytes =null;
            byte[] saltValueBytes = null;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password;
            if (saltValue_config == null || initVector_config == null || passPhrase_config == null)
            {
                initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                password = new PasswordDeriveBytes(passPhrase, saltValueBytes, "MD5", 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(saltValue_config) && !string.IsNullOrEmpty(initVector_config) && !string.IsNullOrEmpty(passPhrase_config))
                {
                    initVectorBytes = Encoding.ASCII.GetBytes(initVector_config);
                    saltValueBytes = Encoding.ASCII.GetBytes(saltValue_config);
                    password = new PasswordDeriveBytes(passPhrase_config, saltValueBytes, "MD5", 1);
                }
                else 
                {
                    initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                    saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                    password = new PasswordDeriveBytes(passPhrase, saltValueBytes, "MD5", 1);
                 
                }

            }
           
            byte[] keyBytes = password.GetBytes(16);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        /// <summary>
        /// Decryption
        /// </summary>
        /// <param name="cipherText">The ciphertext to decrypt</param>
        /// <returns>The plaintext</returns>
        public static string Decrypt(string cipherText)
        {
           // byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            //byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            //byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
           // PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, "MD5", 1);
            byte[] initVectorBytes = null;
            byte[] saltValueBytes = null;
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
           string plainText = "";
           PasswordDeriveBytes password;
           if (saltValue_config == null || initVector_config == null || passPhrase_config == null)
           {
               initVectorBytes = Encoding.ASCII.GetBytes(initVector);
               saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
               password = new PasswordDeriveBytes(passPhrase, saltValueBytes, "MD5", 1);
           }
           else
           {
               if (!string.IsNullOrEmpty(saltValue_config)  && !string.IsNullOrEmpty(initVector_config)   && !string.IsNullOrEmpty(passPhrase_config))
               {
                   initVectorBytes = Encoding.ASCII.GetBytes(initVector_config);
                   saltValueBytes = Encoding.ASCII.GetBytes(saltValue_config);
                   password = new PasswordDeriveBytes(passPhrase_config, saltValueBytes, "MD5", 1);
               }
               else 
               {

                   initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                   saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                   password = new PasswordDeriveBytes(passPhrase, saltValueBytes, "MD5", 1);
               
                }
           }
            try
            {
                byte[] keyBytes = password.GetBytes(16);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (System.Exception ex)
            {
                throw ex;
            
            }
            return plainText;
        }

        public static string GetToken(string userName)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(TokenInitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(TokenSaltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(userName);
            PasswordDeriveBytes password = new PasswordDeriveBytes(TokenPassPhrase, saltValueBytes, "MD5", 1);
            byte[] keyBytes = password.GetBytes(initVector.Length);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

    }
}
