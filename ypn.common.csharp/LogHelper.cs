/**
* 命名空间： ypn.common.csharp
*
* 功    能： N/A
* 类    名： LogHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018-11-20 17:38:01 YPN      初版
*
* Copyright (c) 2018 Fimeson. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：宁夏菲麦森流程控制技术有限公司 　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ypn.common.csharp
{
    public class LogHelper
    {
        /// <summary>
        /// 将异常写入到LOG文件
        /// </summary>
        /// <param name="i_ex">异常</param>
        /// <param name="i_logAddress">日志文件地址</param>
        public static void Write(Exception i_ex, string i_logAddress = "")
        {
            // 如果日志文件为空，则默认在Debug目录下新建 YYYY-MM-DD_Log.log文件
            if (i_logAddress == "")
            {
                i_logAddress = Environment.CurrentDirectory + '\\' +
                DateTime.Now.Year + '-' +
                DateTime.Now.Month + '-' +
                DateTime.Now.Day + "_Log.log";
            }
            // 把异常信息输出到文件
            StreamWriter fs = new StreamWriter(i_logAddress, true);
            fs.WriteLine("当前时间：" + DateTime.Now.ToString());
            fs.WriteLine("异常信息：" + i_ex.Message);
            fs.WriteLine("异常对象：" + i_ex.Source);
            fs.WriteLine("调用堆栈：\n" + i_ex.StackTrace.Trim());
            fs.WriteLine("触发方法：" + i_ex.TargetSite);
            fs.WriteLine();
            fs.Close();
        }
    }
}
