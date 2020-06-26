using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.Helper
{
    public class ElasticSearchHelper
    {
        private string Conn { get { return HcCrm.Util.Config.GetAppsettingsValue("ConnectionString", "Elasticsearch"); } }
        private string DBName { get { return HcCrm.Util.Config.GetAppsettingsValue("Database", "Elasticsearch"); } }
        public ElasticClient client;
        public ElasticSearchHelper()
        {
            //创建客户端
            var uris = new[] { new Uri(Conn) };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool).DefaultIndex(DBName);
            client = new ElasticClient(settings);
        }
        public ElasticSearchHelper(string conn)
        {
            //创建客户端
            var uris = new[] { new Uri(conn) };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool);
            client = new ElasticClient(settings);
        }

        public ElasticSearchHelper(string conn, string dbname)
        {
            //创建客户端
            var uris = new[] { new Uri(conn) };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool).DefaultIndex(dbname);
            client = new ElasticClient(settings);
        }

        #region 索引操作
        /// <summary>
        ///索引是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExistsByElasticClientIndex()
        {
            var existsResponse = client.Indices.Exists(DBName);
            var isexists = existsResponse.Exists;
            return isexists;
        }
        /// <summary>
        ///索引是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExistsByElasticClientIndex(string dbname)
        {
            var existsResponse = client.Indices.Exists(dbname);
            var isexists = existsResponse.Exists;
            return isexists;
        }


        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public bool CreateElasticClientIndex()
        {
            CreateIndexResponse createIndexResponse = client.Indices.Create(DBName);
            var iscreate = createIndexResponse.Acknowledged;
            return iscreate;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public bool CreateElasticClientIndex(string dbname)
        {
            CreateIndexResponse createIndexResponse = client.Indices.Create(dbname);
            var iscreate = createIndexResponse.Acknowledged;
            return iscreate;
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <returns></returns>
        public bool DeleteElasticClientIndex()
        {
            DeleteIndexResponse deleteIndexResponse = client.Indices.Delete(DBName);
            var isdelete = deleteIndexResponse.Acknowledged;
            return isdelete;
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <returns></returns>
        public bool DeleteElasticClientIndex(string dbname)
        {
            DeleteIndexResponse deleteIndexResponse = client.Indices.Delete(dbname);
            var isdelete = deleteIndexResponse.Acknowledged;
            return isdelete;
        }
        #endregion
    }

    public class ElasticSearchExtendHelper<T> where T : class, new()
    {
        public ElasticClient client
        {
            get
            {
                return new ElasticSearchHelper("http://localhost:9200", "zhaoxi").client;
            }
        }



        #region 文档操作
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<int> InsertEntityAsync(T t)
        {
            try
            {
                var task = await client.IndexDocumentAsync(t);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion
    }
}
