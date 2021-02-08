/************************************************************************
*Copyright (c) 2021   All Rights Reserved .
*CLR 版本    ：4.0.30319.42000
*机器名称    ：PC-20201201KGNJ
*公司名称    : 
*命名空间    ：ypn.common.csharp
*文件名称    ：ConvertHelper.cs
*版 本 号    : 2021|V1.0.0.0 
*=================================
*创 建 者    ：@ YANGPIENA
*创建日期    ：2021/02/07 15:40:03 
*电子邮箱    ：yangpiena@163.com
*个人主站    ：http://ynn5ru.coding-pages.com
*功能描述    ：
*使用说明    ：
*=================================
*修改日期    ：2021/02/07 15:40:03 
*修 改 者    ：Administrator
*修改描述    ：
*版 本 号    : 2021|V1.0.0.0 
***********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 
    /// <see cref="类型转换工具类" langword="" />
    /// </summary>
    public class TypeConvertHelper
    {
        #region DataTable转换
        /// <summary>
        /// Datatable转字典列表
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DataTableToDictionary(DataTable dataTable)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    result.Add(dataColumn.ColumnName, dataRow[dataColumn].ToString());
                }
                list.Add(result);
            }
            return list;
        }

        /// <summary>
        /// DataTable转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T DataTableToEntity<T>(DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                    && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);

                        }

                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// DataTable转实体类集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> DataTableToEntityList<T>(DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            if (table == null)
                return null;
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                    && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// DataTable数据筛选
        /// YPN 2018-11-21 Create
        /// </summary>
        /// <param name="i_DataTable">数据表</param>
        /// <param name="i_Distincts">查询/去重的字段数组</param>
        /// <param name="i_Where">查询条件</param>
        /// <param name="i_orderBy">排序字段</param>
        /// <returns></returns>
        /// 
        public static DataTable DataTableFilter(DataTable i_DataTable, string[] i_Distincts, string i_Where, string i_orderBy)
        {
            DataTable v_NewTable = i_DataTable.Clone();
            if (i_DataTable.Rows.Count > 0)
            {
                // 筛选
                DataRow[] v_DataRows = i_DataTable.Select(i_Where);
                foreach (DataRow v_DataRow in v_DataRows)
                {
                    v_NewTable.ImportRow(v_DataRow);
                }
                // 排序
                v_NewTable.DefaultView.Sort = i_orderBy;
                // 去重
                DataView v_DataView = v_NewTable.DefaultView;
                v_NewTable = v_DataView.ToTable(true, i_Distincts);

            }

            return v_NewTable;
        }

        /// <summary>
        /// DataTable数据筛选
        /// YPN 2018-11-21 Create
        /// </summary>
        /// <param name="i_DataTable">数据表</param>
        /// <param name="i_Distincts">查询/去重的字段数组</param>
        /// <param name="i_Where">查询条件</param>
        /// <param name="i_orderBy">排序字段</param>
        /// <returns></returns>
        /// 
        public static DataTable DataTableFilter(DataTable i_DataTable, string i_Where, string i_orderBy)
        {
            DataTable v_NewTable = i_DataTable.Clone();
            if (i_DataTable.Rows.Count > 0)
            {
                // 筛选
                DataRow[] v_DataRows = i_DataTable.Select(i_Where);
                foreach (DataRow v_DataRow in v_DataRows)
                {
                    v_NewTable.ImportRow(v_DataRow);
                }
                // 排序
                v_NewTable.DefaultView.Sort = i_orderBy;
                // 去重
                DataView v_DataView = v_NewTable.DefaultView;
                v_NewTable = v_DataView.ToTable();

            }

            return v_NewTable;
        }

        /// <summary>
        /// DataTable根据指定列去重
        /// </summary>
        /// <param name="i_DataTable">需要去重的datatable</param>
        /// <param name="i_ColNames">依据哪些列去重</param>
        /// <returns></returns>
        public static DataTable DataTableDistinct(DataTable i_DataTable, params string[] i_ColNames)
        {
            DataTable distinctTable = i_DataTable.Clone();
            try
            {
                if (i_DataTable != null && i_DataTable.Rows.Count > 0)
                {
                    DataView dv = new DataView(i_DataTable);
                    distinctTable = dv.ToTable(true, i_ColNames);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return distinctTable;
        }

        /// <summary>
        /// DataTable去重
        /// </summary>
        /// <param name="i_DataTable">需要去重的datatable</param>
        /// <returns></returns>
        public static DataTable DataTableDistinct(DataTable i_DataTable)
        {
            DataTable distinctTable = null;
            try
            {
                if (i_DataTable != null && i_DataTable.Rows.Count > 0)
                {
                    string[] columnNames = GetDataTableColumnName(i_DataTable);
                    DataView dv = new DataView(i_DataTable);
                    distinctTable = dv.ToTable(true, columnNames);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return distinctTable;
        }

        /// <summary>
        /// 获取DataTable中所有列名
        /// </summary>
        /// <param name="i_DataTable"></param>
        /// <returns></returns>
        public static string[] GetDataTableColumnName(DataTable i_DataTable)
        {
            string cols = string.Empty;
            for (int i = 0; i < i_DataTable.Columns.Count; i++)
            {
                cols += (i_DataTable.Columns[i].ColumnName + ",");
            }
            cols = cols.TrimEnd(',');
            return cols.Split(',');
        }
        #endregion


        #region List转换
        /// <summary>
        /// 将指定的集合转换成DataTable
        /// YPN 2019-08-05 Create
        /// </summary>
        /// <param name="list">将指定的集合。</param>
        /// <returns>返回转换后的DataTable。</returns>
        public static DataTable ListToDataTable(IList list)
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
        public static DataTable ListToDataTable<T>(List<T> list)
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
        #endregion

        #region 编码类型转换
        /// UTF8转GB2312
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
        /// GB2312转UTF8
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
        #endregion
    }
}
