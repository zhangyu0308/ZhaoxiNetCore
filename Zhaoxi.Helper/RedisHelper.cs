using HcCrm.Util;
using NewLife.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Zhaoxi.Helper
{
    public class RedisHelper
    {

        #region -- 连接信息 --
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();
        private static NewLife.Caching.Redis prcm;
        private static int retryflag = 0;

        static RedisHelper()
        {
            CreateManager();
        }
        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            try
            {
                FullRedis.Register();
                ThreadPool.SetMinThreads(100, 100);
                prcm = new NewLife.Caching.FullRedis(redisConfigInfo.ReadServerList, redisConfigInfo.RedisDbPassword, redisConfigInfo.RedisDbInt);
                prcm.Expire = 0;//默认缓存时间 0 表示不过期
                //prcm.Log = XTrace.Log;// 调试日志。正式使用时注释掉
                prcm.StartPipeline();
                prcm.AutoPipeline = 10;
                prcm.Timeout = 3000;
                prcm.Retry = 5;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string GetCacheInfo()
        {
            try
            {
                return "NewLifeRedisCache-" + redisConfigInfo.WriteServerList;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static string[] Splitstring(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static NewLife.Caching.Redis GetClient(int newflag = 0)
        {
            try
            {
                if (prcm == null || newflag == 1)
                    CreateManager();

                return prcm;
            }
            catch (Exception e)
            {
                return prcm;
            }
        }
        #endregion

        #region -- Item --
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t)
        {

            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    return redis.Set<T>(key, t);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t, TimeSpan timeSpan)
        {
            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    return redis.Set<T>(key, t, timeSpan);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expire">  过期时间，秒。小于0时采用默认缓存时间NewLife.Caching.Cache.Expire,这里用的是 过期时间-当前时间，剩余的秒数</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t, DateTime expireTime)
        {
            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    TimeSpan ts = expireTime - DateTime.Now;
                    return redis.Set<T>(key, t, ts.TotalSeconds.ToInt());
                }

            }
            catch (Exception e)
            {
                retryflag++;

                if (5 < retryflag)
                {
                    throw e;
                }

                using (NewLife.Caching.Redis redis = GetClient(1))
                {
                    TimeSpan ts = expireTime - DateTime.Now;
                    return redis.Set<T>(key, t, ts.TotalSeconds.ToInt());
                }
            }

        }

        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    return redis.Get<T>(key);
                }
            }
            catch (Exception e)
            {
                retryflag++;

                if (5 < retryflag)
                {
                    throw e;
                }

                using (NewLife.Caching.Redis redis = GetClient(1))
                {
                    return redis.Get<T>(key);
                }
            }
        }
        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectSuscess()
        {
            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    redis.Set<string>("TestRedis", "OK");
                    var redisvalue = redis.Get<string>("TestRedis");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetCacheCount()
        {
            using (NewLife.Caching.Redis redis = GetClient())
            {
                return redis.Count;
            }
        }
        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove(string key)
        {
            using (NewLife.Caching.Redis redis = GetClient())
            {
                return redis.Remove(key.Split(',')) > 0;
            }
        }
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            using (NewLife.Caching.Redis redis = GetClient())
            {
                redis.Clear();
            }
        }
        #endregion

        #region list
        /// <summary>
        /// 添加一个项到内部的List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void List_Add<T>(string key, T t)
        {
            var list = prcm.GetList<T>(key);
            list.Add(t);
        }
        /// <summary>
        /// 移除指定ListId的内部List<T>中第二个参数值相等的那一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool List_Remove<T>(string key, T t)
        {
            var list = prcm.GetList<T>(key);
            var re = list.Remove(t);
            return re;
        }
        /// <summary>
        /// 移除key所有的list对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void List_RemoveAll<T>(string key)
        {
            var list = prcm.GetList<T>(key);
            foreach (var item in list)
            {
                list.Remove(item);
            }
        }
        /// <summary>
        /// 计算key所有的对象数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long List_Count<T>(string key)
        {
            var list = prcm.GetList<T>(key);
            var re = list.Any() ? list.Count() : 0;
            return re;
        }


        /// <summary>
        /// 获取指定ListId的内部List<T>中指定的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> List_GetList<T>(string key)
        {
            var list = prcm.GetList<T>(key);
            return list.ToList();
        }
        #endregion

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetExpire(string key, DateTime datetime)
        {
            using (NewLife.Caching.Redis redis = GetClient())
            {
                TimeSpan ts = datetime - DateTime.Now;
                redis.SetExpire(key, ts);
            }
        }

        /// <summary>
        /// 累加，原子操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static long Increment(string key, long value)
        {
            try
            {
                using (NewLife.Caching.Redis redis = GetClient())
                {
                    return redis.Increment(key, value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 累减，原子操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static long Decrement(string key, long value)
        {
            using (NewLife.Caching.Redis redis = GetClient())
            {
                return redis.Decrement(key, value);
            }
        }

        /// <summary>
        /// 是否启用redis
        /// </summary>
        /// <returns></returns>
        public static bool GetIsEnabled()
        {
            if (Config.GetAppsettingsValue("Enabled", "RedisCaching").ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public sealed class RedisConfigInfo
    {

        public static RedisConfigInfo GetConfig()
        {
            RedisConfigInfo section = new RedisConfigInfo();

            section.WriteServerList = GetValue("WriteServerList");
            section.ReadServerList = GetValue("ReadServerList");
            section.RedisDbPassword = GetValue("RedisDbPassword");
            section.MaxWritePoolSize = GetValue("MaxWritePoolSize").ToInt();
            section.MaxReadPoolSize = GetValue("MaxReadPoolSize").ToInt();
            section.AutoStart = GetValue("AutoStart").ToLower() == "true";
            section.LocalCacheTime = GetValue("LocalCacheTime").ToInt();
            section.RecordeLog = GetValue("RecordeLog").ToLower() == "true";
            section.RedisDbInt = GetValue("RedisDbInt").ToInt();

            return section;
        }
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            try
            {
                var tempvalue = Config.GetAppsettingsValue("Enabled", "RedisCaching").ToLower();
                return string.IsNullOrEmpty(tempvalue) ? "" : tempvalue.Trim();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public string WriteServerList
        { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        public string RedisDbPassword
        { get; set; }

        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        public string ReadServerList
        { get; set; }


        /// <summary>
        /// 最大写链接数
        /// </summary>
        public int MaxWritePoolSize
        { get; set; }


        /// <summary>
        /// 最大读链接数
        /// </summary>
        public int MaxReadPoolSize
        { get; set; }
        /// <summary>
        /// 数据库DB数
        /// </summary>
        public int RedisDbInt
        { get; set; }

        /// <summary>
        /// 自动重启
        /// </summary>
        public bool AutoStart
        { get; set; }



        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        public int LocalCacheTime
        { get; set; }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        public bool RecordeLog
        { get; set; }

    }
}
