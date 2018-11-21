/**
* 命名空间： ypn.common.csharp
*
* 功    能： 验证码工具类
* 类    名： VerifyCodeHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018/10/19 12:11:22 YPN      初版
*
*/
using System;

namespace ypn.common.csharp
{
    /// <summary>
    /// 验证码工具类
    /// </summary>
    public class VerifyCodeHelper
    {
        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        public static string RandomNumber(int i_Length)
        {
            return RandomNumber(i_Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        /// <param name="i_Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string RandomNumber(int i_Length, bool i_Sleep)
        {
            if (i_Sleep) System.Threading.Thread.Sleep(3);
            string v_result = "";
            Random v_Random = new Random();
            for (int i = 0; i < i_Length; i++)
            {
                v_result += v_Random.Next(10).ToString();
            }
            return v_result;
        }
        #endregion

        #region 生成随机数字与字母
        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        public static string RandomString(int i_Length)
        {
            return RandomString(i_Length, false);
        }

        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        /// <param name="i_Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string RandomString(int i_Length, bool i_Sleep)
        {
            if (i_Sleep) System.Threading.Thread.Sleep(3);
            char[] v_Pattern    = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string v_result     = "";
            int    v_PatternLen = v_Pattern.Length;
            Random v_Random     = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < i_Length; i++)
            {
                int v_rnd = v_Random.Next(0, v_PatternLen);
                v_result += v_Pattern[v_rnd];
            }
            return v_result;
        }
        #endregion

        #region 生成随机字母
        /// <summary>
        /// 生成随机字母
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        public static string RandomLetter(int i_Length)
        {
            return RandomLetter(i_Length, false);
        }

        /// <summary>
        /// 生成随机字母
        /// </summary>
        /// <param name="i_Length">生成长度</param>
        /// <param name="i_Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string RandomLetter(int i_Length, bool i_Sleep)
        {
            if (i_Sleep) System.Threading.Thread.Sleep(3);
            char[] v_Pattern    = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string v_result     = "";
            int    v_PatternLen = v_Pattern.Length;
            Random v_Random     = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < i_Length; i++)
            {
                int v_rnd = v_Random.Next(0, v_PatternLen);
                v_result += v_Pattern[v_rnd];
            }
            return v_result;
        }
        #endregion
    }
}
