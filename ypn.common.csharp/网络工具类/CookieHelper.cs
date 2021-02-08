/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：CookieHelper.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/02/01 22:52:04 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/02/01 22:52:04 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Web;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="CookieHelper" langword="" />
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 使用API WinInet 返回Cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookieByWinInet(string url)
        {
            return FullWebBrowserCookie.GetCookieInternal(new Uri(url), true);
        }

        /// <summary>
        /// 按照格式以“；”分行，并以“=”来查看cookie的个数。
        /// </summary>
        /// <param name="cookieStr"></param>
        /// <returns></returns>
        public static CookieContainer GetCookie(string url, string cookieStr)
        {
            CookieContainer myCookieContainer = new CookieContainer();
            string CurHost = new Uri(url).Host;
            //string cookieStr = webBrowser1.Document.Cookie;
            string[] cookstr = cookieStr.Split(';');

            string flag = "";
            foreach (string str in cookstr)
            {
                if (str.IndexOf("_saltkey") != -1)
                {
                    string[] cookieNameValue = str.Split('=');
                    flag = cookieNameValue[0].Replace("_saltkey", "").Trim();
                }
            }
            //
            //MessageBox.Show(flag);
            //flag = "";
            myCookieContainer.PerDomainCapacity = 40;

            foreach (string str in cookstr)
            {
                try
                {
                    string[] cookieNameValue = str.Split('=');
                    string strvalue = cookieNameValue[1].Trim().ToString().Replace(",", "%2C");
                    strvalue = str.Replace(cookieNameValue[0] + "=", "");
                    strvalue = strvalue.Trim().ToString().Replace(",", "%2C");

                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), strvalue);
                    ck.Domain = CurHost;
                    myCookieContainer.Add(ck);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message + ">>\r\n" + cookieStr); }
            }

            return myCookieContainer;
        }
    }

    #region API WinInet
    /// <summary>
    /// 利用在浏览器地址栏输入javascript:alert (document. cookie)的方法取不到HttpOnly的cookie，所以使用wininetAPI能够取得完整cookie并且可以根据你想要的格式返回给你。
    /// 构造获取cookie的类，首先把url转为string，获取访问url的权限，然后利用wininet下的InternetGetCookieEx获取cookie，返回为string格式。
    /// </summary>
    public class FullWebBrowserCookie
    {
        /// <summary>
        /// 从托管代码中访问非托管DLL函数之前，需要知道该函数的名称以及该DLL的名称，然后为DLL的非托管函数编写托管定义。
        ///它将用到static和extern修饰符，此类型的公共静态成员对于多线程操作是安全的。DllImport属性提供非托管DLL函数的调用信息。
        /// </summary>
        internal sealed class NativeMethods
        {
            #region enums

            public enum ErrorFlags
            {
                ERROR_INSUFFICIENT_BUFFER = 122,
                ERROR_INVALID_PARAMETER = 87,
                ERROR_NO_MORE_ITEMS = 259
            }

            public enum InternetFlags
            {
                INTERNET_COOKIE_HTTPONLY = 8192, //Requires IE 8 or higher
                INTERNET_COOKIE_THIRD_PARTY = 131072,
                INTERNET_FLAG_RESTRICTED_ZONE = 16
            }

            #endregion enums

            #region DLL Imports

            [SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("wininet.dll", EntryPoint = "InternetGetCookieExW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
            internal static extern bool InternetGetCookieEx([In] string Url, [In] string cookieName, [Out] StringBuilder cookieData, [In, Out] ref uint pchCookieData, uint flags, IntPtr reserved);

            #endregion DLL Imports
        }

        [SecurityCritical]
        public static string GetCookieInternal(Uri uri, bool throwIfNoCookie)
        {
            uint pchCookieData = 0;
            string url = UriToString(uri);
            uint flag = (uint)NativeMethods.InternetFlags.INTERNET_COOKIE_HTTPONLY;

            //获取 string builder的大小
            if (NativeMethods.InternetGetCookieEx(url, null, null, ref pchCookieData, flag, IntPtr.Zero))
            {
                pchCookieData++;
                StringBuilder cookieData = new StringBuilder((int)pchCookieData);

                //读取cookie
                if (NativeMethods.InternetGetCookieEx(url, null, cookieData, ref pchCookieData, flag, IntPtr.Zero))
                {
                    DemandWebPermission(uri);
                    return cookieData.ToString();
                }
            }
            //返回由上一个非托管函数返回的错误代码调用的dll文件函数
            int lastErrorCode = Marshal.GetLastWin32Error();

            if (throwIfNoCookie || (lastErrorCode != (int)NativeMethods.ErrorFlags.ERROR_NO_MORE_ITEMS))
            {
                throw new Win32Exception(lastErrorCode);
            }

            return null;
        }

        private static void DemandWebPermission(Uri uri)
        {
            string uriString = UriToString(uri);

            if (uri.IsFile)
            {
                string localPath = uri.LocalPath;
                new FileIOPermission(FileIOPermissionAccess.Read, localPath).Demand();
                //如果未对调用堆栈中处于较高位置的所有调用方授予当前实例所指定的权限，则在运行时强制SecurityException
            }
            else
            {
                new WebPermission(NetworkAccess.Connect, uriString).Demand();
            }
        }

        //URI转string
        private static string UriToString(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            UriComponents components = (uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString);//获取绝对url
            return new StringBuilder(uri.GetComponents(components, UriFormat.SafeUnescaped), 2083).ToString();
        }
    }
    #endregion
}
