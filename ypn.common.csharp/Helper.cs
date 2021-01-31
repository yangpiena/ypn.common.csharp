using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ypn.common.csharp
{
    /// <summary>
    /// 帮助工具类
    /// </summary>
    public class Helper
    {
        public static double DoubleParse(string str)
        {
            double.TryParse(str,out double value);
            return value;
        }
        public static Decimal ChangeDataToD(string strData)
        {
            decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                double b = DoubleParse(strData.ToUpper().Split('E')[0].ToString());//整数部分
                double c = DoubleParse(strData.ToUpper().Split('E')[1].ToString());//指数部分
                b = Math.Round(b, 3);
                dData = Convert.ToDecimal(b * Math.Pow(10, c));
            }
            else
            {
                dData = Convert.ToDecimal(strData);
            }
            return dData;
        }
        /// <summary>
        /// 判断输入的数是否是科学计数法。如果是的话，就会将其换算成整数并且返回，否则就返回false。
        /// </summary>
        /// <param name="num"></param>
        /// <param name="CompleteNum"></param>
        /// <returns></returns>
        public static void ConvertNum(string a, out double result)
        {
            result = -1;
            if (a.ToUpper().Contains("E"))
            {
                double b = DoubleParse(a.ToUpper().Split('E')[0].ToString());//整数部分
                double c = DoubleParse(a.ToUpper().Split('E')[1].ToString());//指数部分
                result = b;
            }
            else
            {
                result = DoubleParse(a);
            }
        }
      public  static Int32 GetDecimalNum(Double num)
        {
            Int32 result = 0;
            Double newNum = 0d;

            do
            {
                newNum = num * Math.Pow(10, result);

                if ((Int32)newNum == newNum)
                {
                    break;
                }

            } while (++result < Int32.MaxValue - 1);

            return result;
        }
        ///MMQ Create 2019-08-13
        /// <summary>
        /// 按格式获取序列号 例如 1,2-3,8,4-9
        /// </summary>
        /// <param name="no_list">序列号集合</param>
        /// <returns></returns>
        public static string GetSeriesNum(List<int> no_list)
        {
            string needExportNo = "";
            List<List<int>> res = new List<List<int>>();
            var context = new Context();
            foreach (var i in no_list.OrderBy(p => p))
            {
                if (!context.a.HasValue)
                {
                    context.a = i;
                    context.list.Add(context.a.Value);
                    continue;
                }
                context.b = i;
                if (context.b - context.a == 1)
                {

                    context.list.Add(context.b.Value);
                    context.a = i;

                }
                else
                {
                    res.Add(context.list);
                    context = new Context();
                    context.a = i;
                    context.list.Add(i);

                }
            }
            res.Add(context.list);
            foreach (var item in res)
            {
                if (item.Count > 1)
                {
                    needExportNo += item[0] + "-" + item[item.Count - 1] + ",";
                }
                else
                {
                    needExportNo += item[0] + ",";
                }

            }
            return needExportNo;
        }
        class Context
        {
            public int? a;
            public int? b;
            public List<int> list = new List<int>();
        }
    }
}
