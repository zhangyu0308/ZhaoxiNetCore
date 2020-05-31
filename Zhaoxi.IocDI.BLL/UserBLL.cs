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
    }
}
