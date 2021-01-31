using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ypn.common.csharp
{
    /// <summary>
    /// 验证类工具类
    /// </summary>
    public class ValidateHelper
    {
        #region 验证TextBox的输入内容，仅限输入正整数
        /// <summary>
        /// 验证TextBox的输入内容，仅限输入正整数
        /// YPN Create 2018-09-28
        /// </summary>
        /// <param name="r_TextBox"></param>
        public static void ValidateInteger(ref TextBox r_TextBox)
        {
            Regex v_Regex = new Regex(@"^[1-9]\d*$");
            string v_str = r_TextBox.Text;

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    v_TmpText += v_str[i].ToString();
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        #endregion

        #region 验证ComboBox的输入内容，仅限输入正整数
        /// <summary>
        /// 验证ComboBox的输入内容，仅限输入正整数
        /// YPN Create 2018-09-28
        /// </summary>
        /// <param name="r_ComboBox"></param>
        public static void ValidateInteger(ComboBox r_ComboBox)
        {
            Regex v_Regex = new Regex(@"^[1-9]\d*$");
            string v_str = r_ComboBox.Text;

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    v_TmpText += v_str[i].ToString();
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_ComboBox.Text = v_NewText;
                r_ComboBox.SelectionStart = r_ComboBox.Text.Length;
            }
        }
        #endregion
        #region 验证ComboBox的输入内容，仅限输入正整数
        /// <summary>
        /// 验证TextBox的输入内容，仅限输入整数
        /// MMQ Create 2020-02-26
        /// </summary>
        /// <param name="r_TextBox"></param>
        public static void ValidateRound(ref TextBox r_TextBox)
        {
            Regex v_Regex = new Regex(@"^[0-9]\d*$");
            string v_str = r_TextBox.Text;

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    v_TmpText += v_str[i].ToString();
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        #endregion
        #region 验证TextBox的输入内容，仅限输入浮点数
        /// <summary>
        /// 验证TextBox的输入内容，仅限输入浮点数
        /// YPN Create 2018-09-28
        /// </summary>
        /// <param name="r_TextBox">目标TextBox</param>
        public static void ValidateFloat(ref TextBox r_TextBox)
        {
            Regex v_Regex = new Regex(@"^(-)?(\d+)?(\d+\.)?(\d+)?$");
            string v_str = r_TextBox.Text.Trim();

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    //根据美国用户习惯如果输入.554,自动让他变成0.554
                    if (v_str[0].ToString() == ".")
                    {
                        v_TmpText += "0.";
                    }
                    else
                    {
                        v_TmpText += v_str[i].ToString();
                    }
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        #endregion

        #region 验证TextBox的输入内容，仅限输入正浮点数
        /// <summary>
        /// 验证TextBox的输入内容，仅限输入正浮点数
        /// YPN Create 2018-09-28
        /// </summary>
        /// <param name="r_TextBox">目标TextBox</param>
        public static void ValidatePositiveFloat(ref TextBox r_TextBox)
        {
         
            Regex v_Regex = new Regex(@"^(\d+)(\.)?(\d+)?$");
            string v_str = r_TextBox.Text.Trim();

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    //根据美国用户习惯如果输入.554,自动让他变成0.554
                    if (v_str[0].ToString()==".")
                    {
                        v_TmpText += "0.";
                    }
                    else
                    {
                      v_TmpText += v_str[i].ToString();
                    }
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        public static void ValidatePositiveFloatForCbo(ref ComboBox r_TextBox)
        {

            Regex v_Regex = new Regex(@"^(\d+)(\.)?(\d+)?$");
            string v_str = r_TextBox.Text.Trim();

            if (!v_Regex.IsMatch(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    //根据美国用户习惯如果输入.554,自动让他变成0.554
                    if (v_str[0].ToString() == ".")
                    {
                        v_TmpText += "0.";
                    }
                    else
                    {
                        v_TmpText += v_str[i].ToString();
                    }
                    if (v_Regex.IsMatch(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        public static void ValidateSeniceFloat(ref TextBox r_TextBox)
        {

            string v_str = r_TextBox.Text.Trim();

            if (!StringHelper.IsNumeric(v_str))
            {
                string v_TmpText = string.Empty;
                string v_NewText = string.Empty;

                for (int i = 0; i < v_str.Length; i++)
                {
                    //根据美国用户习惯如果输入.554,自动让他变成0.554
                    if (v_str[0].ToString() == ".")
                    {
                        v_TmpText += "0.";
                    }
                    else
                    {
                        v_TmpText += v_str[i].ToString();
                    }
                    if (StringHelper.IsNumeric(v_TmpText))
                    {
                        v_NewText = v_TmpText;
                    }
                }
                r_TextBox.Text = v_NewText;
                r_TextBox.SelectionStart = r_TextBox.Text.Length;
            }
        }
        #endregion
        #region 验证邮箱
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="i_TextBox"></param>
        /// <returns></returns>
        public static bool IsEmail(TextBox i_TextBox)
        {
            Regex v_Regex = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (v_Regex.IsMatch(i_TextBox.Text))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region 验证电话号码
        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="i_TextBox"></param>
        /// <returns></returns>
        public static bool IsTelPhone(TextBox i_TextBox)
        { 
        
            Regex v_Regex1 = new Regex(@"^(\d{3,4}-)?\d{6,8}$");
            Regex v_Regex2=  new Regex(@"^[1]+[3,5,7,8,9]+\d{9}");
            if (v_Regex1.IsMatch(i_TextBox.Text)|| (v_Regex2.IsMatch(i_TextBox.Text)&& i_TextBox.Text.Length==11))
            {
                return true;
            }
            else
            {
                return false;
            }
                    
          
        }
        #endregion
        #region 检查文件名称是否合法
        /// <summary>
        /// 检查文件名称是否合法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckFileName(string fileName)
        {
            bool isValid = true;
            string[] strList = new string[] { "/", "\"", @"\", @"\/", ":", "*", "?", "<", ">", "|", "\r\n" };//" ",

            if (string.IsNullOrEmpty(fileName))
            {
                isValid = false;
            }
            else
            {
                foreach (string errStr in strList)
                {
                    if (fileName.Contains(errStr))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        #endregion
        #region 检查是否是纯数字
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
        }
        #endregion

    }
}
