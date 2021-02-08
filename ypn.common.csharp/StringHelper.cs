/**
* 命名空间： ypn.common.csharp
*
* 功    能： 字符串工具类
* 类    名： StringHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018/10/17 12:24:31 YPN      初版
*
*/
using System;
using System.Collections;

namespace ypn.common.csharp
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="i_Obj">要验证的对象</param>
        public static bool IsNull(object i_Obj)
        {
            // 如果为null
            if (i_Obj == null)
            {
                return true;
            }
            // 如果为""
            if (i_Obj.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(i_Obj.ToString().Trim()) || string.IsNullOrWhiteSpace(i_Obj.ToString()))
                {
                    return true;
                }
            }
            // 如果为DBNull
            if (i_Obj.GetType() == typeof(DBNull))
            {
                return true;
            }
            // 不为空
            return false;
        }

        /// <summary>
        /// 判断字符串是不是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            bool bReturn = true;
            double result;
            try
            {
                result = double.Parse(str);
            }
            catch
            {
                result = 0;
                bReturn = false;
            }
            return bReturn;
        }

        /// <summary>
        /// true：存在重复 false：不存在重复
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool ExistRepeat(string[] array)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < array.Length; i++)
            {
                if (ht.Contains(array[i]))
                {
                    return true;
                }
                else
                {
                    ht.Add(array[i], array[i]);
                }
            }
            return false;
        }

        /// <summary>  
        /// 去除字符串里的空格、回车、换行、Tab符等
        /// </summary>  
        /// <param name="i_Control"></param>  
        /// <returns></returns>  
        public static string TrimStr(string i_str)
        {
            return i_str.Replace("\n", "").Replace("\t", "").Replace("\r", "").Trim();
        }
    }
}
