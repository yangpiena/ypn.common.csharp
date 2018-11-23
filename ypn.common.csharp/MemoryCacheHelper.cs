/**
* 命名空间： xx
*
* 功    能： 基于MemoryCache的缓存辅助类
* 类    名： MemoryCacheHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018-11-21 12:23:15 YPN      初版
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
using System.Runtime.Caching;
using System.Text;

namespace ypn.common.csharp
{
    public static class MemoryCacheHelper
    {
        private static readonly Object _locker = new object(); //线程同步

        /// <summary>
        /// 判断缓存中是否存在指定元素的缓存项
        /// </summary>
        /// <param name="i_key">指定元素的键值</param>
        /// <returns></returns>
        public static bool Contains(string i_key)
        {
            return MemoryCache.Default.Contains(i_key);
        }

        /// <summary>
        /// 获取Catch元素
        /// </summary>
        /// <typeparam name="T">所获取元素的类型</typeparam>
        /// <param name="i_key">元素的键值</param>
        /// <returns>特定的元素值</returns>
        public static T GetCacheItem<T>(String i_key)
        {
            if (string.IsNullOrWhiteSpace(i_key))     throw new ArgumentException("不合法的key!");
            if (!MemoryCache.Default.Contains(i_key)) throw new ArgumentException("获取失败,不存在该key!");
            if (!(MemoryCache.Default[i_key] is T))   throw new ArgumentException("未找到所需类型数据!");

            return (T)MemoryCache.Default[i_key];
        }

        /// <summary>
        /// 添加Catch元素
        /// </summary>
        /// <param name="i_key">元素的键值</param>
        /// <param name="i_value">元素的值</param>
        /// <param name="slidingExpiration">元素过期时间(时间间隔)</param>
        /// <param name="absoluteExpiration">元素过期时间(绝对时间)</param>
        /// <returns></returns>
        public static bool SetCacheItem(string i_key, object i_value, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            var item = new CacheItem(i_key, i_value);
            var policy = CreatePolicy(slidingExpiration, absoluteExpiration);
            lock (_locker)
            {
                return MemoryCache.Default.Add(item, policy);
            }
        }

        /// <summary>
        /// 移出Cache元素
        /// </summary>
        /// <typeparam name="T">待移出元素的类型</typeparam>
        /// <param name="i_key">待移除元素的键</param>
        /// <returns>已经移出的元素</returns>
        public static T RemoveCacheItem<T>(string i_key)
        {
            if (string.IsNullOrWhiteSpace(i_key))     throw new ArgumentException("不合法的key!");
            if (!MemoryCache.Default.Contains(i_key)) throw new ArgumentException("获取失败,不存在该key!");
            if (!(MemoryCache.Default[i_key] is T))   throw new ArgumentException("未找到所需类型数据!");

            return (T)MemoryCache.Default.Remove(i_key);
        }

        /// <summary>
        /// 移出多条缓存数据,默认为所有缓存
        /// </summary>
        /// <typeparam name="T">待移出的缓存类型</typeparam>
        /// <param name="i_keyList"></param>
        /// <returns></returns>
        public static List<T> RemoveAllCacheItem<T>(IEnumerable<string> i_keyList = null)
        {
            if (i_keyList != null)
            {
                return (from key in i_keyList
                        where MemoryCache.Default.Contains(key)
                        where MemoryCache.Default.Get(key) is T
                        select (T)MemoryCache.Default.Remove(key)
                        ).ToList();
            }
            while (MemoryCache.Default.GetCount() > 0)
            {
                MemoryCache.Default.Remove(MemoryCache.Default.ElementAt(0).Key);
            }

            return new List<T>();
        }

        /// <summary>
        /// 设置过期信息
        /// MemoryCache提供了以下三种缓存过期的方式：
        /// 1.绝对到期（指定在一个固定的时间点到期）
        /// 2.滑动到期（在一个时间长度内没有被命中则过期）
        /// 3.到期Token（自定义过期）
        /// </summary>
        /// <param name="slidingExpiration">滑动到期时间</param>
        /// <param name="absoluteExpiration">绝对到期时间</param>
        /// <returns></returns>
        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }
            policy.Priority = CacheItemPriority.Default;

            return policy;
        }
    }
}