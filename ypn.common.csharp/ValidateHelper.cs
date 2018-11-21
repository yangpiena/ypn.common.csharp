/**
* 命名空间： ypn.common.csharp
*
* 功    能： N/A
* 类    名： ValidateHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018-11-20 20:50:19 YPN      初版
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ypn.common.csharp
{
    public class ValidateHelper
    {

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
        /// <summary>
        /// 验证ComboBox的输入内容，仅限输入正整数
        /// YPN Create 2018-09-28
        /// </summary>
        /// <param name="r_TextBox"></param>
        public static void ValidateInteger(ref ComboBox r_ComboBox)
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
    }
}
