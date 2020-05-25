using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Zhaoxi.IocDI.IDAL;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.DAL
{
    public class UserDAL : IUserDAL
    {
        public UserModel Find(Expression<Func<UserModel, bool>> expression)
        {
            return new UserModel() {
                Id = 7,
                Name = "wwe",
                Account = "123",
                Email = "123@123.com",
                Password = "456",
                Role="admin",
                LoginTime=DateTime.Now
            };
        }

        public void Update(UserModel userModel)
        {
            Console.WriteLine("数据库更新");
        }
    }
}
