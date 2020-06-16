using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Zhaoxi.Helper
{
    public interface IMongoDBHelper<T> where T : class, new()
    {
        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null);
        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<List<T>> FindListAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null);
        /// <summary>
        /// 根据id查询单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindOneAsync(string id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        Task<int> AddOnecAsync(T Model);
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        Task<int> AddManyAsync(List<T> Model);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        Task<UpdateResult> EditAsync(string id, T Model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> Where);
    }


    /// <summary>
    /// mongodb帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoDBHelper<T> : IMongoDBHelper<T> where T : class, new()
    {
        private string Conn { get { return HcCrm.Util.Config.GetAppsettingsValue("ConnectionString"); } }
        private MongoClient MongoDBClient;
        private IMongoCollection<T> collection;
        private IMongoDatabase MongoDBDatebase;
        private string DBName { get { return HcCrm.Util.Config.GetAppsettingsValue("Database"); } }

        public MongoDBHelper()
        {
            MongoDBClient = new MongoClient(Conn);
            MongoDBDatebase = MongoDBClient.GetDatabase(DBName);
            Type type = typeof(T);
            collection = MongoDBDatebase.GetCollection<T>(type.Name.ToLower());
        }
        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (sort == null)
                    {
                        return await collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

                    }
                    //进行排序
                    return await collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }
                //制定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                //不排序
                if (sort == null)
                {
                    return await collection.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                //排序查询
                return await collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<List<T>> FindListAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null)
        {
            try
            {
                if (sort == null)
                {
                    return await collection.Find(filter).ToListAsync();
                }
                return await collection.Find(filter).Sort(sort).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据id查询单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> FindOneAsync(string account)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("Account", account);
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<int> AddOnecAsync(T Model)
        {
            try
            {
                await collection.InsertOneAsync(Model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<int> AddManyAsync(List<T> Model)
        {
            try
            {
                await collection.InsertManyAsync(Model);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<UpdateResult> EditAsync(string account, T Model)
        {
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("Account", account);
                //要修改的字段
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in Model.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(Model)));
                }
                var updatefilter = Builders<T>.Update.Combine(list);
                return await collection.UpdateOneAsync(filter, updatefilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> Where)
        {
            try
            {
                return await collection.DeleteOneAsync<T>(Where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
