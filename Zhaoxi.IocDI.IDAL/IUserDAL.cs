using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.IDAL
{
    public interface IUserDAL
    {
        UserModel Find(Expression<Func<UserModel, bool>> expression);
        void Update(UserModel userModel);

        List<user> FindAll();
        void AddOnec(user Model);
        void Edit(string Account, UpdateDefinition<user> Model);
        void Delete(string Account);
    }
}
