/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：Global.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/01/23 19:55:55 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/01/23 19:55:55 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="Global" langword="" />
    /// </summary>
    internal static class Global
    {
        /// <summary>
        /// 缓存过期时间（小时）
        /// </summary>
        public static readonly double CacheDeadline = 1;
        /// <summary>
        /// 系统当前语言
        /// </summary>
        public static string Language { get; set; } = "zh-CN";
    }
}
