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
        List<T> FindAsync(Expression<Func<T, bool>> Query);
        List<T> AllAsync();
        void AddOnec(T Model);
        void AddMany(params T[] Model);
        void Edit(Expression<Func<T, bool>> Where, UpdateDefinition<T> Model);
        void Delete(Expression<Func<T, bool>> Where);
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
            collection = MongoDBDatebase.GetCollection<T>(type.Name);
        }

        public List<T> FindAsync(Expression<Func<T, bool>> Query)
        {
            return collection.Find(Query).ToList();
        }

        public List<T> AllAsync()
        {
            return collection.Find(x => true).ToList();
        }

        public void AddOnec(T Model)
        {
            collection.InsertOne(Model);
        }
        public void AddMany(params T[] Model)
        {
            collection.InsertMany(Model);
        }

        public void Edit(Expression<Func<T, bool>> Where, UpdateDefinition<T> Model)
        {
            collection.UpdateOne<T>(Where, Model);
        }

        public void Delete(Expression<Func<T, bool>> Where)
        {
            collection.DeleteOne<T>(Where);
        }
    }
}
