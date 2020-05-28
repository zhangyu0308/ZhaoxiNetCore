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
        public UserModel Find(Expression<Func<UserModel, bool>> expression)
        {
            return new UserModel()
            {
                Id = "www",
                Name = "wwe",
                Account = "123",
                Email = "123@123.com",
                Password = "456",
                Role = "admin",
                LoginTime = DateTime.Now
            };
        }
        /// <summary>
        /// new MongoDBHelper<UserModel>() 应该使用单例设计模式
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<user>> FindAll()
        {

            try
            {
                var list = await new MongoDBHelper<user>().AllAsync();
                return list;
            }
            catch (Exception ex)
            {
                var list = await new MongoDBHelper<user>().AllAsync();
                return list;
            }

        }

        public void Update(UserModel userModel)
        {
            Console.WriteLine("数据库更新");
        }
    }
}
