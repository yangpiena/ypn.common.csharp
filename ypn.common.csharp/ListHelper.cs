using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ypn.common.csharp
{
    /// <summary>
    /// 集合工具类
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// 将指定的集合转换成DataTable
        /// YPN 2019-08-05 Create
        /// </summary>
        /// <param name="list">将指定的集合。</param>
        /// <returns>返回转换后的DataTable。</returns>
        public static DataTable ToDataTable(this IList list)
        {
            DataTable table = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Type pt = pi.PropertyType;
                    if ((pt.IsGenericType) && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        pt = pt.GetGenericArguments()[0];
                    }
                    table.Columns.Add(new DataColumn(pi.Name, pt));
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }

        /// <summary>
        /// 将指定类型的集合转换成指定类型的DataTable
        /// YPN 2019-08-05 Create
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable table = new DataTable();
            //创建列头
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                Type pt = pi.PropertyType;
                if ((pt.IsGenericType) && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    pt = pt.GetGenericArguments()[0];
                }
                table.Columns.Add(new DataColumn(pi.Name, pt));
            }
            //创建数据行
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }

        ///MMQ Create 2019-08-05
        /// <summary>
        /// 自定义Distinct扩展方法
        /// </summary>
        /// <typeparam name="T">要去重的对象类</typeparam>
        /// <typeparam name="C">自定义去重的字段类型</typeparam>
        /// <param name="source">要去重的对象</param>
        /// <param name="getfield">获取自定义去重字段的委托</param>
        /// <returns></returns>
        public static IEnumerable<T> List_Distinct<T, C>(this IEnumerable<T> source, Func<T, C> getfield)
        {
            return source.Distinct(new Compare<T, C>(getfield));
        }
        ///MMQ Create 2019-7-31
        /// <summary>bv                                                                      
        /// 拆分List
        /// </summary>
        /// <param name="tagetlist">要拆分的list</param>
        /// <param name="size">拆分的大小</param>
        /// <returns></returns>
        public static List<List<string>> CreateList(List<string> tagetlist, int size)
        {
            List<List<string>> listArr = new List<List<string>>();
            //获取被拆分的个数
            int arrsize = tagetlist.Count % size == 0 ? tagetlist.Count / size : (tagetlist.Count / size) + 1;
            for (int i = 0; i < arrsize; i++)
            {
                List<string> templist = new List<string>();
                for (int j = i * size; j < size * (i + 1); j++)
                {
                    if (j <= tagetlist.Count - 1)
                    {
                        templist.Add(tagetlist[j]);
                    }
                }
                listArr.Add(templist);
            }
            return listArr;
        }
        /// MMQ Create 2019-08-05
        /// <summary>
        /// List转Datatable
        /// </summary>
        /// <typeparam name="T">要转换的对象</typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private class Compare<T, C> : IEqualityComparer<T>
        {
            private Func<T, C> _getField;
            public Compare(Func<T, C> getfield)
            {
                this._getField = getfield;
            }
            public bool Equals(T x, T y)
            {
                return EqualityComparer<C>.Default.Equals(_getField(x), _getField(y));
            }
            public int GetHashCode(T obj)
            {
                return EqualityComparer<C>.Default.GetHashCode(this._getField(obj));
            }
        }
    }
}
