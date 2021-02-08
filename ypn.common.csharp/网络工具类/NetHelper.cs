/**
* 命名空间： ypn.common.csharp
*
* 功    能： 网络相关的工具类
* 类    名： NetHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2019/08/22 16:10:59 MMQ      初版
* 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;

namespace ypn.common.csharp
{
    /// <summary>
    /// 网络相关的工具类
    /// </summary>
    public class NetHelper
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);
       
        /// <summary>
        /// 用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }
       
        /// <summary>
        /// 用于判断服务器是否连接正常
        /// </summary>
        /// <returns></returns>
        public static bool IsServerConnectInternet()
        {
            try
            {
                WebRequest myRequest = WebRequest.Create("http://218.21.3.26:5042/xx/webServlet");
                myRequest.Timeout = 3000;
                WebResponse myResponse = myRequest.GetResponse();
                myResponse.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
       
        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
