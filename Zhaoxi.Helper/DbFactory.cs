using HcCrm.Util;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zhaoxi.Helper
{
    //根据配置文件切换数据库
    public interface IDbFactory
    {
        DapperClient GetDapperClient();
    }
    public class DbFactory: IDbFactory
    {
        private static string dbtype = Config.GetValue("DBServerType");
        private static ConnectionConfig GetConnectionConfig()
        {
            ConnectionConfig connectionConfig = new ConnectionConfig();
            switch (dbtype)
            {
                case "sqlserver":
                    connectionConfig.DbType = DbStoreType.SqlServer;
                    connectionConfig.ConnectionString = Config.GetAppsettingsValue("SqlServerMainConn", "ConnectionStrings");
                    break;
                default:
                    connectionConfig.DbType = DbStoreType.MySql;
                    connectionConfig.ConnectionString = Config.GetAppsettingsValue("MySqlMainConn", "ConnectionStrings");
                    break;
            }
            return connectionConfig;
        }
        public DapperClient GetDapperClient()
        {
            var DapperClient = new DapperClient(GetConnectionConfig());
            return DapperClient;
        }
    }
    /// <summary>
    /// 链接信息
    /// </summary>
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }
        public DbStoreType DbType { get; set; }
    }
    /// <summary>
    /// 数据库枚举
    /// </summary>
    public enum DbStoreType
    {
        /// <summary>
        /// 数据库类型：SqlServer
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// 数据库类型：MySql
        /// </summary>
        MySql = 1,
        /// <summary>
        /// 数据库类型：MongoDB
        /// </summary>
        MongoDB = 2,
        /// <summary>
        /// 数据库类型：Oracle
        /// </summary>
        Oracle = 3
    }

    public class DapperClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClient(ConnectionConfig config) { CurrentConnectionConfig = config; }

        IDbConnection _connection = null;
        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    case DbStoreType.MySql:
                        _connection = new MySqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.SqlServer:
                        _connection = new SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    //case DbStoreType.Sqlite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;

                    //case DbStoreType.Oracle:
                    //    _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, null).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSql);
                return res.FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>0成功，-1执行失败</returns>
        public virtual int Execute(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strSql, param) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, null, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
