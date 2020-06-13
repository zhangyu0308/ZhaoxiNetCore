using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.IBLL
{
   public interface IUserBLL
    {
        UserModel Login(string account);
        void LastLogin(UserModel user);

        /// <summary>
        /// Mongodb
        /// </summary>
        /// <returns></returns>
        List<user> FindAll();
        /// <summary>
        /// Mongodb
        /// </summary>
        /// <returns></returns>
        void AddOnec(user Model);
        /// <summary>
        /// Mongodb
        /// </summary>
        /// <returns></returns>
        void Edit(string Account, UpdateDefinition<user> Model);
        /// <summary>
        /// Mongodb
        /// </summary>
        /// <returns></returns>
        void Delete(string Account);
    }
}
