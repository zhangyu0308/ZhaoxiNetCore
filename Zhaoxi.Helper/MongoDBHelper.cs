using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using MongoDB.Driver;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Zhaoxi.Helper
{

    #region Connect
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
        Connect()
        {
            this.Dispose(false);
        }
    }
    #endregion

    #region MongoDBHelper
    public interface IMongoDBHelper<T> where T : class, new()
    {
        Task<T> FindAsync(Expression<Func<T, bool>> Query);
        Task<IEnumerable<T>> AllAsync();
        Task<T> AddAsync(T Model);
        Task<ReplaceOneResult> EditAsync(Expression<Func<T, bool>> Where, T Model);
        Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> Where);

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
        public string CollectionName { get; set; }

        public string ConnectionStr = GetAppsettingsValue("ConnectionString");
        public string DefaultDataBaseName = GetAppsettingsValue("Database");

        public MongoDBHelper()
        {
            //typeof(T)  获取类型的System.Type 对象(包含类中的所有方法、属性、字段)
            CollectionNameAttribute mongoCollectionName = (CollectionNameAttribute)typeof(T).GetTypeInfo().GetCustomAttribute(typeof(CollectionNameAttribute));
            this.CollectionName = (mongoCollectionName != null ? mongoCollectionName.Name : typeof(T).Name.ToLower());
            this.Connect = new Connect(ConnectionStr, DefaultDataBaseName);
            this.Collection = this.Connect.Collection<T>(this.CollectionName);
        }

        public async Task<IEnumerable<T>> AllAsync()
        {
            IFindFluent<T, T> findFluent = IMongoCollectionExtensions.Find<T>(this.Collection, new BsonDocument(), null);
            return await IAsyncCursorSourceExtensions.ToListAsync<T>(findFluent, new CancellationToken());
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            IFindFluent<T, T> findFluent = IMongoCollectionExtensions.Find<T>(this.Collection, where, null);
            return await IFindFluentExtensions.FirstOrDefaultAsync<T, T>(findFluent, new CancellationToken());
        }

        public async Task<T> AddAsync(T model)
        {
            await this.Collection.InsertOneAsync(model, null, new CancellationToken());
            return model;
        }
        public async Task<ReplaceOneResult> EditAsync(Expression<Func<T, bool>> where, T model)
        {
            IMongoCollection<T> collection = this.Collection;
            FilterDefinition<T> filterDefinition = where;
            T t = model;
            ReplaceOptions replaceOptions = new ReplaceOptions() { IsUpsert = true };
            CancellationToken cancellationToken = new CancellationToken();
            return await collection.ReplaceOneAsync(filterDefinition, t, replaceOptions, cancellationToken);
        }

        public async Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await this.Collection.DeleteOneAsync(where, new CancellationToken());
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the CollectionName class attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the collection.</param>
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty collection name is not allowed", nameof(value));

            Name = value;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public virtual string Name { get; }
    }
    #endregion



    public class MongoDBHelperTest<T> where T : class, new()
    {
        private string Conn { get { return HcCrm.Util.Config.GetAppsettingsValue("ConnectionString"); } }
        private MongoClient MongoDBClient;
        private IMongoCollection<T> collection;
        private IMongoDatabase MongoDBDatebase;
        private string DBName { get { return HcCrm.Util.Config.GetAppsettingsValue("Database"); } }

        public MongoDBHelperTest()
        {
            MongoDBClient = new MongoClient(Conn);
            MongoDBDatebase = MongoDBClient.GetDatabase(DBName);
            Type type = typeof(T);
            collection = MongoDBDatebase.GetCollection<T>(type.Name);
        }

        public IMongoQueryable<T> FindAsync(Expression<Func<T, bool>> Query)
        {
            return collection.AsQueryable<T>().Where(Query);
        }

        public IMongoQueryable<T> AllAsync()
        {
            return collection.AsQueryable<T>();
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
