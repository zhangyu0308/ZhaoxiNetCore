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

        public void LastLogin(UserModel user)
        {
            user.LoginTime = DateTime.Now;
            this._userDAL.Update(user);
        }

        public UserModel Login(string account)
        {
            return this._userDAL.Find(u => u.Account.Equals(account));
        }

        public void AddOnec(user Model)
        {
            this._userDAL.AddOnec(Model);
        }

        public void Delete(string Account)
        {
            this._userDAL.Delete(Account);
        }

        public void Edit(string Account, UpdateDefinition<user> Model)
        {
            this._userDAL.Edit(Account, Model);
        }

        public List<user> FindAll()
        {
            return this._userDAL.FindAll();
        }
    }
}
