using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.IocDI.IBLL;
using Zhaoxi.IocDI.IDAL;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.BLL
{
    public class UserBLL : IUserBLL
    {
        private IUserDAL _userDAL;
        public UserBLL(IUserDAL userDAL)
        {
            this._userDAL = userDAL;
        }

        public List<user> FindListByPage(FilterDefinition<user> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<user> sort = null)
        {
            return this._userDAL.FindListByPage(filter, pageIndex, pageSize, field, sort);
        }
        public List<user> FindListAsync(FilterDefinition<user> filter, SortDefinition<user> sort = null)
        {
            return this._userDAL.FindListAsync(filter, sort);
        }
        public user FindOne(string account)
        {
            return this._userDAL.FindOne(account);
        }
        public bool AddOnec(user Model)
        {
            return this._userDAL.AddOnec(Model);
        }

        public bool Delete(string account)
        {
            return this._userDAL.Delete(account);
        }

        public bool Edit(string account, user Model)
        {
            return this._userDAL.Edit(account, Model);
        }
    }
}
