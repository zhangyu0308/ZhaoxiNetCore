using HcCrm.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.Helper;
using Zhaoxi.IocDI.IDAL;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.DAL
{
    public class UserDAL : IUserDAL
    {
        #region mongoDBHelper
        private IMongoDBHelper<user> mongoDBHelper = new MongoDBHelper<user>();

        public List<user> FindListByPage(FilterDefinition<user> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<user> sort = null)
        {
            return mongoDBHelper.FindListByPageAsync(filter, pageIndex, pageSize, field, sort).Result;
        }
        public List<user> FindListAsync(FilterDefinition<user> filter, SortDefinition<user> sort = null)
        {
            return mongoDBHelper.FindListAsync(filter, sort).Result;
        }
        public user FindOne(string account)
        {
            return mongoDBHelper.FindOneAsync(account).Result;
        }
        public bool AddOnec(user Model)
        {
            Model.Id = ObjectId.GenerateNewId().ToString();
            return mongoDBHelper.AddOnecAsync(Model).Result > 0 ? true : false;
        }

        public bool Delete(string account)
        {
            return mongoDBHelper.DeleteAsync(x => x.Account == account).Result.DeletedCount > 0 ? true : false;
        }

        public bool Edit(string account, user Model)
        {
            return mongoDBHelper.EditAsync(account, Model).Result.ModifiedCount > 0 ? true : false;
        }
        #endregion

        #region Dapper获取数据
        public DapperClient GetDapperClient
        {
            get
            {
                IDbFactory dbFactory = new DbFactory();
                return dbFactory.GetDapperClient();
            }
        }

        public List<user> GetDapperList()
        {
            return GetDapperClient.Query<user>("select * from user where 1=1");
        }
        #endregion
    }
}
