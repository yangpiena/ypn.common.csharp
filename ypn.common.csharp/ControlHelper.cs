/**
* 命名空间： ypn.common.csharp
*
* 功    能： 控件相关的工具类
* 类    名： ControlHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018/10/10 00:19:59 YPN      初版
* 
*/
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ypn.common.csharp
{
    public class ControlHelper
    {
        #region 自动消失的 MessageBox
        [DllImport("user32.dll")]
        static extern int MessageBoxTimeoutA(int hWnd, string lpText, string lpCaption, int uType, int wLanguageId, int dwMilliseconds);
        /// <summary>
        /// 自动消失的消息窗口
        /// </summary>
        /// <param name="i_Text">消息内容</param>
        /// <param name="i_Caption">消息标题</param>
        /// <param name="i_Milliseconds">显示时间（毫秒）</param>
        public static void MessageBoxTimeout(string i_Text, string i_Caption, int i_Milliseconds)
        {
            MessageBoxTimeoutA(0, i_Text, i_Caption, 0, 0, i_Milliseconds);
        }
        #endregion

        #region 获取和设置 webBrowser 的内核IE版本
        /// <summary>
        /// 修正WebBrowser控件的浏览器内核版本
        /// </summary>
        /// <param name="processName">待修正的进程名：System.Diagnostics.Process.GetCurrentProcess().ProcessName</param>
        public static void SetWebBrowserVersion(string processName)
        {
            try
            {
                int browserVersion, registerValue;

                // get the installed IE version
                using (var webBrowser = new System.Windows.Forms.WebBrowser())
                {
                    browserVersion = webBrowser.Version.Major;
                }

                // set the appropriate IE version
                if (browserVersion >= 11) registerValue = 11001;
                else if (browserVersion == 10) registerValue = 10001;
                else if (browserVersion == 9) registerValue = 9999;
                else if (browserVersion == 8) registerValue = 8888;
                else registerValue = 7000;

                // set the actual key
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true))
                {
                    if (key != null)
                    {
                        //key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", registerValue, Microsoft.Win32.RegistryValueKind.DWord);
                        key.SetValue(processName + ".exe", registerValue, Microsoft.Win32.RegistryValueKind.DWord);
                        key.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 修改注册表信息来兼容当前程序
        /// </summary>
        public static void SetWebBrowserFeatures(int ieVersion)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //获取程序及名称
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //得到浏览器的模式的值
            UInt32 ieMode = GeoEmulationModee(ieVersion);
            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION",
                appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION",
                appName, 1, RegistryValueKind.DWord);
        }

        /// <summary>
        /// 获取IE浏览器的版本
        /// </summary>
        /// <returns></returns>
        public static int GetIEBrowserVersion()
        {
            int browserVersion = 0;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree,
                System.Security.AccessControl.RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }
            //如果小于7
            if (browserVersion < 7)
            {
                throw new ApplicationException("不支持的浏览器版本!");
            }
            return browserVersion;
        }

        /// <summary>
        /// 获取控件WebBrowser的版本
        /// </summary>
        /// <returns></returns>
        public static string GetWebBrowserVersion()
        {
            string version = (new WebBrowser()).Version.Major.ToString();
            return version;
        }

        /// <summary>
        /// 通过版本得到浏览器模式的值
        /// </summary>
        /// <param name="browserVersion"></param>
        /// <returns></returns>
        private static UInt32 GeoEmulationModee(int browserVersion)
        {
            UInt32 mode = 11000; // Internet Explorer 11
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Internet Explorer 7
                    break;
                case 8:
                    mode = 8000; // Internet Explorer 8
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10.
                    break;
                case 11:
                    mode = 11000; // Internet Explorer 11
                    break;
            }
            return mode;
        }

        /// <summary>
        /// 查询系统环境是否支持IE8以上版本
        /// </summary>
        public static bool IfWindowsSupport()
        {
            bool isWin7 = Environment.OSVersion.Version.Major > 6;
            bool isSever2008R2 = Environment.OSVersion.Version.Major == 6
                && Environment.OSVersion.Version.Minor >= 1;

            if (!isWin7 && !isSever2008R2)
            {
                return false;
            }
            else return true;
        }
        #endregion
    }
}
