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
using System.Runtime.InteropServices;

namespace ypn.common.csharp
{
    public class ControlHelper
    {
        #region 自动消失的消息窗口
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
    }
}
