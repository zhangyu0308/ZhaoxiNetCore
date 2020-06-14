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
        List<user> FindListByPage(FilterDefinition<user> filter, int pageIndex, int pageSize, string[] field = null, SortDefinition<user> sort = null);
        List<user> FindListAsync(FilterDefinition<user> filter, SortDefinition<user> sort = null);
        user FindOne(string account);
        bool AddOnec(user Model);
        bool Edit(string account, user Model);
        bool Delete(string account);
    }
}
