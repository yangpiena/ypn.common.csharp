/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：ConfigHelper.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/01/26 21:40:05 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/01/26 21:40:05 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="ConfigHelper" langword="" />
    /// </summary>
    public class ConfigHelper
    {
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置标识</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            string result = string.Empty;
            if (config.AppSettings.Settings[key] != null)
                result = config.AppSettings.Settings[key].Value;
            return result;
        }

        /// <summary>
        /// 修改或增加配置值
        /// </summary>
        /// <param name="key">配置标识</param>
        /// <param name="value">配置值</param>
        public static void SetValue(string key, string value)
        {
            if (config.AppSettings.Settings[key] != null)
                config.AppSettings.Settings[key].Value = value;
            else
                config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 删除配置值
        /// </summary>
        /// <param name="key">配置标识</param>
        public static void DeleteValue(string key)
        {
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
