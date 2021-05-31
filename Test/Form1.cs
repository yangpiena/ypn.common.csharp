using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YPN = ypn.common.csharp;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = YPN.EncodeHelper.AESEncode(textBox1.Text, "2021#03#01 14:01", "kpic@2021#aes123");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = YPN.EncodeHelper.AESDecode(textBox3.Text, "2021#03#01 14:01", "kpic@2021#aes123");
        }

    }
}
