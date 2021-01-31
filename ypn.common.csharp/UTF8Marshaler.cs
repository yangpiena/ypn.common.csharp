using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ypn.common.csharp
{
 
    public class EncodeConvert 
    {
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UTF8ToGB2312(string str)
        {
            try
            {
                Encoding utf8 = Encoding.UTF8;
                Encoding gb2312 = Encoding.GetEncoding("gb2312");//Encoding.Default ,936
                byte[] temp = utf8.GetBytes(str);
                byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);
                string result = gb2312.GetString(temp1);
                return result;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GB2312ToUTF8(string str)
        {
            try
            {
                Encoding uft8 = Encoding.GetEncoding(65001);
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] temp = gb2312.GetBytes(str);
               
                byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
              
                string result = uft8.GetString(temp1);
                return result;
            }
            catch (Exception)//(UnsupportedEncodingException ex)
            {
                return null;
            }
        }
    }
}
