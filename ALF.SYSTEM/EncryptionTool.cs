using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ALF.SYSTEM
{
    /// <summary>
    /// 
    /// </summary>
    public static class EncryptionTool
    {
        /// <summary>
        /// Get the ASCII of the letter
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static int GetAscIi(string str)
        {
            return Encoding.ASCII.GetBytes(str)[0];
        }

        /// <summary>
        /// Tell whether the letter is a correct letter in the alphabet  or not
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsChar(string s)
        {
            return "abcdefghijklmnopqrstuvwxyz".Contains(s.ToLower());
        }

        /// <summary>
        /// Tell whether the letter is upper or not
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsUpper(string s)
        {
            return s.ToUpper() == s;
        }


        #region Symmentric Cipher
        /// <summary>
        /// Function of Symmetric Encryption
        /// </summary>
        /// <param name="originalMessage">The message needs to be encrpypted</param>
        /// <param name="keyInt">Key for Ceasar Encryption, while using Vigenere Encryption, make it 0</param>
        /// <param name="keyString">Key for Vigenere Encryption, while using Ceasar Encryption, make it ""</param>
        /// <returns>Encrpyted message</returns>
        public static string SymmetriEncrypt(string originalMessage, int keyInt, string keyString)
        {
            var result = "";
            var keylength = keyString.Length;
            var keytoint = new int[keyString.Length];
            for (var i = 0; i < keylength; i++)
            {
                keytoint[i] = keyString.ToLower()[i] - 'a';
            }

            for (var i = 0; i < originalMessage.Length; i++)
            {
                var key = keyInt;
                if (keylength != 0)
                {
                    key = keytoint[i % keylength];
                }

                var origLetter = originalMessage[i].ToString();

                if (!IsChar(origLetter))
                {
                    result += origLetter;
                    continue;
                }
                var offset = (GetAscIi(origLetter.ToLower()) + key - GetAscIi("a")) % 26;
                var encryptLetter = Convert.ToChar(offset + GetAscIi("a")).ToString(CultureInfo.InvariantCulture);
                if (IsUpper(origLetter))
                {
                    encryptLetter = encryptLetter.ToUpper();
                }
                result += encryptLetter;
            }
            return result;
        }

        /// <summary>
        /// Function of Symmetric Decryption
        /// </summary>
        /// <param name="encrpyMessage">The message needs to be decrpypted</param>
        /// <param name="keyInt">Key for Ceasar Encryption, while using Vigenere Encryption, make it 0</param>
        /// <param name="keyString">Key for Vigenere Encryption, while using Ceasar Encryption, make it ""</param>
        /// <returns>Decrypted message</returns>
        public static string SymmetricDecrypt(string encrpyMessage, int keyInt, string keyString)
        {
            var result = "";
            var keylength = keyString.Length;
            var keytoint = new int[keyString.Length];
            for (var i = 0; i < keylength; i++)
            {
                keytoint[i] = keyString.ToLower()[i] - 'a';
            }

            for (var i = 0; i < encrpyMessage.Length; i++)
            {
                var key = keyInt;
                if (keylength != 0)
                {
                    key = keytoint[i % keylength];
                }

                var encryptLetter = encrpyMessage[i].ToString();
                if (!IsChar(encryptLetter))
                {
                    result += encryptLetter;
                    continue;
                }
                var offset = (GetAscIi("z") + key - GetAscIi(encryptLetter.ToLower())) % 26;
                var origLetter = Convert.ToChar(GetAscIi("z") - offset).ToString(CultureInfo.InvariantCulture);
                if (IsUpper(encryptLetter))
                {
                    origLetter = origLetter.ToUpper();
                }
                result += origLetter;
            }
            return result;

        }
        #endregion


        #region Hash
        /// <summary>
        /// Generate the key for MD5
        /// </summary>
        /// <returns>The key for MD5</returns>
        public static string GenerateKey()
        {
            var desCrypto = (DESCryptoServiceProvider)DES.Create();
            return Encoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// MD5 Encryption
        /// </summary>
        /// <param name="orignialMessage">The message needs to be encrpypted</param>
        /// <param name="md5Key">The key for MD5 encryption</param>
        /// <returns>Encrpyted message</returns>
        public static string Md5Encrypt(string orignialMessage, string md5Key)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Encoding.Default.GetBytes(orignialMessage);
                des.Key = Encoding.ASCII.GetBytes(md5Key);
                des.IV = Encoding.ASCII.GetBytes(md5Key);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in Alf.SYSTEM:加密报错[{0}]", exception.Message);
                return "";
            }
        }

        /// <summary>
        /// MD5 Decryption
        /// </summary>
        /// <param name="encryptedMessage">The message needs to be decrpypted</param>
        /// <param name="md5Key">The key for MD5 decryption</param>
        /// <returns>Decrpyted message</returns>
        public static string Md5Decrypt(string encryptedMessage, string md5Key)
        {
            try
            {
                var des = new DESCryptoServiceProvider();

                var inputByteArray = new byte[encryptedMessage.Length / 2];
                for (var x = 0; x < encryptedMessage.Length / 2; x++)
                {
                    try
                    {
                        
                        var i = (Convert.ToInt32(encryptedMessage.Substring(x * 2, 2), 16));
                        inputByteArray[x] = (byte)i;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                des.Key = Encoding.ASCII.GetBytes(md5Key);
                des.IV = Encoding.ASCII.GetBytes(md5Key);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Encoding.Default.GetString(ms.ToArray());

            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in Alf.SYSTEM:解密报错[{0}]", exception.Message);
                return "";
            }
        }
        #endregion


        #region RSA
        /// <summary>
        /// Generate the key for rsa encryption and decryption
        /// </summary>
        /// <param name="xmlPrivateKey">private key for rsa decryption</param>
        /// <param name="xmlPublicKey">public key for rsa encryption</param>
        public static void RsaKey(out string xmlPrivateKey, out string xmlPublicKey)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                xmlPrivateKey = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception)
            {
                xmlPrivateKey = "";
                xmlPublicKey = "";
            }
        }

        /// <summary>
        /// RSA Encryption
        /// </summary>
        /// <param name="xmlPublicKey">public key for rsa encryption</param>
        /// <param name="encryptString">The message needs to be encrpypted</param>
        /// <returns>Encrpyted message</returns>
        public static string RsaEncrypt(string xmlPublicKey, string encryptString)
        {
            var result = "";
            try
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                var plainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
                var cypherTextBArray = rsa.Encrypt(plainTextBArray, false);
                result = Convert.ToBase64String(cypherTextBArray);
                return result;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return result;
        }

        /// <summary>
        /// RSA Decryption
        /// </summary>
        /// <param name="xmlPrivateKey">private key for rsa decryption</param>
        /// <param name="decryptString">The message needs to be decrpypted</param>
        /// <param name="decryptSuccess">Whether the decryption succeed</param>
        /// <returns>Decrpyted message</returns>
        public static string RsaDecrypt(string xmlPrivateKey, string decryptString, out bool decryptSuccess)
        {
            decryptSuccess = false;
            var result = "";
            try
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                var plainTextBArray = Convert.FromBase64String(decryptString);
                var dypherTextBArray = rsa.Decrypt(plainTextBArray, false);
                result = (new UnicodeEncoding()).GetString(dypherTextBArray);
                decryptSuccess = true;
                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return result;
        }
        #endregion
    }  
}
