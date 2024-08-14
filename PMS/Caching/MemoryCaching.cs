using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace PMS.Caching
{
    public static class MemoryCaching
    {
        public static bool CacheKeyExist(string key)
        {
            MemoryCache cache = MemoryCache.Default;
            return cache.Contains(key);
        }
        public static object GetCacheValue(string key)
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(key))
                return cache.Get(key);
            else
                return null;
        }

        public static void AddCacheValue(string key, object value)
        {
            MemoryCache cache = MemoryCache.Default;
            cache.Add(key, value, DateTimeOffset.UtcNow.AddHours(12));
        }

        public static object GetOrAddCacheValue(string key, object value)
        {
            MemoryCache cache = MemoryCache.Default;
            if (!CacheKeyExist(key))
            {
                cache.Add(key, value, DateTimeOffset.UtcNow.AddHours(12));
            }

            return cache.Get(key);
        }

        public static void RemoveCacheValue(string key)
        {
            MemoryCache cache = MemoryCache.Default;
            if (CacheKeyExist(key))
                cache.Remove(key);
        }

        public static void ClearAllCache()
        {
            MemoryCache cache = MemoryCache.Default;
            List<string> cacheKeys = cache.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                cache.Remove(cacheKey);
            }
        }


    }

    enum CachingKeys
    {
        GetAllNewGrade=0,
        GetAllNewDepartment =1,       
        GetAllNewDesignation = 2,
        GetAllNewOutlet = 3,
        GetAllNewHeads=4,
        GetAllNewSubHeads = 5,
        GetCachedHeadName=6,
        GetAllNewEmployee = 7,
        GetAllNewUser = 8,

    }
}