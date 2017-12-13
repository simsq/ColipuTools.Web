using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colipu.Common.Library
{
    public class AesHelper
    {

        /// <summary>
        /// 默认密钥-密钥的长度必须是32
        /// </summary>
        private const string PublicKey = "1qaz2wsx3edc4rfv";

        /// <summary>
        /// 默认向量
        /// </summary>
        private const string Iv = "abcdefghijklmnop";
        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="str">需要加密字符串</param>  
        /// <returns>加密后字符串</returns>  
        public static String Encrypt(string str)
        {
            try
            {
                return Encrypt(str, PublicKey);
            }
            catch (Exception e)
            {
                return str;
            }
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="str">需要解密字符串</param>  
        /// <returns>解密后字符串</returns>  
        public static String Decrypt(string str)
        {
            try
            {
                return Decrypt(str, PublicKey);
            }
            catch (Exception e)
            {
                return str;
            }
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = System.Text.Encoding.UTF8.GetBytes(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>  
        /// AES加密 
        /// </summary>  
        /// <param name="str">需要加密字符串</param>  
        /// <returns>加密后字符串</returns>  
        public static String EncryptToString(string str)
        {
            try
            {
                return EncryptToString(str, PublicKey);
            }
            catch (Exception e)
            {
                return str;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptToString(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = System.Text.Encoding.UTF8.GetBytes(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            StringBuilder builder = new StringBuilder();
            int num2 = resultArray.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                builder.AppendFormat("{0:X2}", resultArray[i]);
            }
            return builder.ToString();
            // return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="str">需要解密字符串</param>  
        /// <returns>解密后字符串</returns>  
        public static String DecryptToString(string str)
        {
            try
            {
                return DecryptToString(str, PublicKey);
            }
            catch (Exception e)
            {
                return str;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptToString(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            int halfInputLength = str.Length / 2;
            Byte[] toEncryptArray = new Byte[halfInputLength];
            for (int x = 0; x < halfInputLength; x++)
            {
                int i = (Convert.ToInt32(str.Substring(x * 2, 2), 16));
                toEncryptArray[x] = (byte)i;
            }

            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
            //  return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
