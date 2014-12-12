//======================================================================
//
// Copyright (C) 2007-2008 深圳清华天安
// All rights reserved
// 本  机:TIANAN-KEVIN
// 文件名:SetCache
// 描  述:
// 创建者:龙康才 
// 时  间:2008-12-15 17:23:49
//
//====================================================================== 
using System;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Text.RegularExpressions;

namespace SMT
{
    /// <summary>
    /// 设置Cache操作类
    /// </summary>
    public class SMTCache
    {
        #region 用户自定义变量
        private static readonly Cache _cache;//缓存实例
        private static readonly int hourfactor;
        #endregion

        #region 构造函数
        static SMTCache()
        {
            hourfactor = 3600;
            _cache = HttpRuntime.Cache;
        }

        private SMTCache()
        {
        }
        #endregion

        #region 清除所有缓存
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            //要循环访问 Cache 对象的枚举数
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();//检索用于循环访问包含在缓存中的键设置及其值的字典枚举数
            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    _cache.Remove(enumerator.Key.ToString());
                }
            }
        }
        #endregion

        #region 得到缓存实例
        /// <summary>
        /// 得到缓存实例
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <returns>返回缓存实例</returns>
        public static object GetCache(string key)
        {
            return _cache[key];
        }
        #endregion

        #region 缓存实例插入
        /// <summary>
        /// 缓存实例插入(默认缓存20分钟)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        public static void Insert(string key, object obj)
        {
            CacheDependency dep = null;
            Insert(key, obj, dep, 20);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="seconds">缓存的时间</param>
        public static void Insert(string key, object obj, int seconds)
        {
            CacheDependency dep = null;
            Insert(key, obj, dep, seconds);
        }

        /// <summary>
        /// 缓存实例插入(缓存过期时间是一天)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="dep">缓存的依赖项</param>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, hourfactor * 12);
        }

        /// <summary>
        /// 缓存实例插入(缓存过期时间是一天)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="xmlPath">缓存的依赖项xml文件的路径（绝对路径）</param>
        public static void Insert(string key, object obj, string xmlPath)
        {
            CacheDependency dep = new CacheDependency(xmlPath);
            Insert(key, obj, dep, hourfactor * 12);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="seconds">缓存时间</param>
        /// <param name="priority">该对象相对于缓存中存储的其他项的成本</param>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="dep">该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含空引用（Visual Basic 中为 Nothing）</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="xmlPath">缓存的依赖项xml文件的路径（绝对路径）</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        public static void Insert(string key, object obj, string xmlPath, int seconds)
        {
            CacheDependency dep = new CacheDependency(xmlPath);
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="dep">该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含空引用（Visual Basic 中为 Nothing）。</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        /// <param name="priority">该对象相对于缓存中存储的其他项的成本，由 CacheItemPriority 枚举表示。该值由缓存在退出对象时使用；具有较低成本的对象在具有较高成本的对象之前被从缓存移除。 </param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds((double)seconds), TimeSpan.Zero, priority, null);
            }
        }
        #endregion

        #region 移出单个缓存
        /// <summary>
        /// 移出单个缓存
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
        #endregion

        #region 得到所有使用的Cache键值
        /// <summary>
        /// 得到所有使用的Cache键值
        /// </summary>
        /// <returns>返回所有的Cache键值</returns>
        public static ArrayList GetAllCacheKey()
        {
            ArrayList arrList = new ArrayList();
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    arrList.Add(enumerator.Key);
                }
            }
            return arrList;
        }
        #endregion
        private bool _IsDispose = false;
        
        #region 初始化
        /// <summary>
        /// 垃圾回收
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放过资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._IsDispose)
            {
                if (disposing)
                {
                    GC.Collect();
                }
            }
            _IsDispose = true;

        }
        /// <summary>
        /// 析构
        /// </summary>
        ~SMTCache()
        {
            Dispose(false);
        }
        #endregion
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns></returns>
        public static bool IsExist(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if(objCache.Get(cacheKey)!=null)
            {
                return true;
            }
            else{
                return false;
            }
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="objPackage">缓存内容</param>
        public static void SetCache(string cacheKey, object objPackage)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objPackage);
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="objPackage">缓存内容</param>
        /// <param name="intMinute">过期时间 分钟</param>
        public static void SetCache(string cacheKey, object objPackage,int intMinute)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objPackage,null,DateTime.Now.AddMinutes(intMinute),TimeSpan.Zero);
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <returns></returns>
        public static object GetCacheValue(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }
        
        /// <summary>
        /// 移除指定CacheKey的值
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        public static void RemoveCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(cacheKey);
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void CacheClear()
        { 
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            System.Collections.IDictionaryEnumerator cacheEnumer = objCache.GetEnumerator();
            while (cacheEnumer.MoveNext())
            {
                objCache.Remove(cacheEnumer.Key.ToString());
            }
        }

    }
}