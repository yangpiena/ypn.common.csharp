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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    public class StringHelper
    {
        #region 判断对象是否为空，为空返回true
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="i_Obj">要验证的对象</param>
        public static bool IsNullOrEmpty(object i_Obj)
        {
            //如果为null
            if (i_Obj == null)
            {
                return true;
            }

            //如果为""
            if (i_Obj.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(i_Obj.ToString().Trim()) || string.IsNullOrWhiteSpace(i_Obj.ToString()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (i_Obj.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion
    }
}
