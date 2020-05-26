using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using MongoDB.Driver;

namespace Zhaoxi.Helper
{

    public interface IConnect : IDisposable
    {
        MongoDatabaseSettings DatabaseSettings
        {
            get; set;
        }

        IMongoCollection<T> Collection<T>(string collectionName);
    }
    [Serializable]
    public class Connect : IDisposable, IConnect
    {
        private bool disposed;

        public MongoClient Client
        {
            get;
            set;
        }

        public IMongoDatabase DataBase
        {
            get;
            set;
        }

        public MongoDatabaseSettings DatabaseSettings
        {
            get; set;
        }
        public Connect(string connectionString, string databaseName)
        {
            this.Client = new MongoClient(connectionString);
            this.DataBase = this.Client.GetDatabase(databaseName, null);

        }

        public Connect(string connectionString, string databaseName, MongoDatabaseSettings databaseSettings)
        {
            this.Client = new MongoClient(connectionString);
            this.DataBase = this.Client.GetDatabase(databaseName, databaseSettings);
        }

        public IMongoCollection<T> Collection<T>(string collectionName)
        {
            return this.DataBase.GetCollection<T>(collectionName, null);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DataBase = null;
                    this.Client = null;
                }
                this.disposed = true;
            }
        }
        ~Connect()
        {
            this.Dispose(false);
        }
    }

    public interface IMongoDBHelper<T> where T : class, new()
    {
        int Add(T entity);
    }
    public class MongoDBHelper<T> : IMongoDBHelper<T> where T : class, new()
    {
        //需要导入Microsoft.AspNetCore包
        public static IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        /// <summary>
        /// 根据Key取Value值   appsettings.json 的值
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppsettingsValue(string keyname, string keypath = "MongoDB")
        {
            try
            {
                return configuration.GetSection(keypath).GetSection(keyname).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public IConnect Connect { get; set; }
        public IMongoCollection<T> Collection { get; set; }

        public string ConnectionStr = GetAppsettingsValue("ConnectionString");
        public string DefaultDataBaseName = GetAppsettingsValue("Database");

        public MongoDBHelper()
        {
            this.Connect = new Connect(ConnectionStr, DefaultDataBaseName);
           // this.Collection = this.Connect.Collection<T>(this.CollectionName);
        }

        public int Add(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
