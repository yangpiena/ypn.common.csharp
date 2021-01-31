using System;
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
            if (!YPN.StringHelper.IsNull(pictureBox1.Image))
            {
                pictureBox1.Image.Dispose();
            }
            if (this.openFileDialog1.ShowDialog().Equals(DialogResult.OK))
            {
                string newImage = System.AppDomain.CurrentDomain.BaseDirectory + "ypn.png";
                File.Delete(newImage);
                label1.Text = YPN.ImageConvertPSD.PSD2PNG(openFileDialog1.FileName, newImage);
                if (File.Exists(newImage))
                {
                    pictureBox1.Image = Image.FromFile(newImage);
                }
            }
        }
    }
}
