/**
* 命名空间： ypn.common.csharp
*
* 功    能： N/A
* 类    名： DataTableHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018-11-21 16:45:15 YPN      初版
*
* Copyright (c) 2018 Fimeson. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：宁夏菲麦森流程控制技术有限公司 　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ypn.common.csharp
{
    public static class DataTableHelper
    {
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
        /// YPN Create 2018-11-21
        /// </summary>
        /// <param name="i_DataTable">数据表</param>
        /// <param name="i_Distincts">查询/去重的字段数组</param>
        /// <param name="i_Where">查询条件</param>
        /// <param name="i_orderBy">排序字段</param>
        /// <returns></returns>
        /// 
        public static DataTable DataFilter(DataTable i_DataTable, string[] i_Distincts, string i_Where, string i_orderBy)
        {
            // 筛选
            DataTable v_NewTable = i_DataTable.Clone();
            DataRow[] v_DataRows = i_DataTable.Select(i_Where, i_orderBy);
            foreach (DataRow v_DataRow in v_DataRows)
            {
                v_NewTable.ImportRow(v_DataRow);
            }
            // 去重
            DataView v_DataView = v_NewTable.DefaultView;
            v_NewTable = v_DataView.ToTable(true, i_Distincts);
            
            return v_NewTable;
        }
    }
}
