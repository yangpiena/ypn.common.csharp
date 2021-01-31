using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    /// <summary>
    /// 表格工具类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 表格转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable table) where T : new()
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
        /// 表格转实体类集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToEntities<T>(this DataTable table) where T : new()
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
        /// 数据筛选
        /// YPN 2018-11-21 Create
        /// </summary>
        /// <param name="i_DataTable">数据表</param>
        /// <param name="i_Distincts">查询/去重的字段数组</param>
        /// <param name="i_Where">查询条件</param>
        /// <param name="i_orderBy">排序字段</param>
        /// <returns></returns>
        /// 
        public static DataTable DataFilter(DataTable i_DataTable, string[] i_Distincts, string i_Where, string i_orderBy)
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
        /// 数据筛选
        /// YPN 2018-11-21 Create
        /// </summary>
        /// <param name="i_DataTable">数据表</param>
        /// <param name="i_Distincts">查询/去重的字段数组</param>
        /// <param name="i_Where">查询条件</param>
        /// <param name="i_orderBy">排序字段</param>
        /// <returns></returns>
        /// 
        public static DataTable DataFilter(DataTable i_DataTable, string i_Where, string i_orderBy)
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
        #region datatable去重
        /// <summary>
        /// datatable去重
        /// </summary>
        /// <param name="dtSource">需要去重的datatable</param>
        /// <param name="columnNames">依据哪些列去重</param>
        /// <returns></returns>
        public static DataTable GetDistinctTable(DataTable dtSource, params string[] columnNames)
        {
            DataTable distinctTable = dtSource.Clone();
            try
            {
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtSource);
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
        /// datatable去重
        /// </summary>
        /// <param name="dtSource">需要去重的datatable</param>
        /// <returns></returns>
        public static DataTable GetDistinctTable(DataTable dtSource)
        {
            DataTable distinctTable = null;
            try
            {
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    string[] columnNames = GetTableColumnName(dtSource);
                    DataView dv = new DataView(dtSource);
                    distinctTable = dv.ToTable(true, columnNames);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return distinctTable;
        }

        #endregion

        #region 获取表中所有列名
        public static string[] GetTableColumnName(DataTable dt)
        {
            string cols = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cols += (dt.Columns[i].ColumnName + ",");
            }
            cols = cols.TrimEnd(',');
            return cols.Split(',');
        }
        #endregion
    }
}
