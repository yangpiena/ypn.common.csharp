/**
* 命名空间： ypn.common.csharp
*
* 功    能： 加密解密工具类
* 类    名： EncryptHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018/10/11 09:36:08 YPN      初版
*
*/
using System;
using System.Security.Cryptography;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 加密解密工具类
    /// </summary>
    public class EncryptHelper
    {
        private static Encoding Encoding_UTF8 = Encoding.UTF8;

        #region 32位MD5加密
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="i_str"></param>
        /// <returns></returns>
        public static string EncryptMD532(string i_str)
        {
            string v_md5Str = "";
            MD5    v_md5    = MD5.Create();                                     // 实例化一个md5对像                                    　
            byte[] v_byte   = v_md5.ComputeHash(Encoding.UTF8.GetBytes(i_str)); // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择

            // 通过循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < v_byte.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                v_md5Str = v_md5Str + v_byte[i].ToString("X2");
            }
            return v_md5Str;
        }
        #endregion

        /// <summary>AES加密</summary>  
        /// <param name="text">明文</param>  
        /// <param name="key">密钥,长度为16的字符串</param>  
        /// <param name="iv">偏移量,长度为16的字符串</param>  
        /// <returns>密文</returns>  
        public static string EncodeAES(string text, string key, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.Zeros;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>AES解密</summary>  
        /// <param name="text">密文</param>  
        /// <param name="key">密钥,长度为16的字符串</param>  
        /// <param name="iv">偏移量,长度为16的字符串</param>  
        /// <returns>明文</returns>  
        public static string DecodeAES(string text, string key, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.Zeros;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }
    }
}
