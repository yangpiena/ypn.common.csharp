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
using System.Security.Cryptography;
using System.Text;

namespace ypn.common.csharp
{
    public class EncryptHelper
    {
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
    }
}
