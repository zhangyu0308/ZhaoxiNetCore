using System;
using System.Collections.Generic;
using System.Text;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.IBLL
{
   public interface IUserBLL
    {
        UserModel Login(string account);
        void LastLogin(UserModel user);
    }
}
