using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using YPN = ypn.common.csharp;

namespace Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.textBox1.Text);//打开网页
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CookieContainer bwCookie = YPN.CookieHelper.GetCookie(textBox1.Text, this.webBrowser1.Document.Cookie);
            CookieContainer realCookie = YPN.CookieHelper.GetCookie(textBox1.Text, YPN.CookieHelper.GetCookieByWinInet(this.textBox1.Text));///api获取

            textBox2.Text = "\r\n DocumentCookies(" + bwCookie.Count.ToString() + "):\r\n" + this.webBrowser1.Document.Cookie + "\r\n "
                                        + "\r\n拆分查看详情：\r\n***" + bwCookie.GetCookieHeader(new Uri(this.textBox1.Text)).Replace(";", ";\r\n***");

            textBox3.Text = "\r\n 真实cookies(" + realCookie.Count.ToString() + "):\r\n" + YPN.CookieHelper.GetCookieByWinInet(this.textBox1.Text) + "\r\n"
                              + "\r\n拆分查看详情：\r\n***" + realCookie.GetCookieHeader(new Uri(this.textBox1.Text)).Replace(";", ";\r\n***");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url.ToString() != textBox1.Text.Trim())
            {
                //if (MessageBox.Show("已经登录,是否关闭此窗口", "退出窗口", MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                //    this.Close();
                //}
            }
        }
    }
}
