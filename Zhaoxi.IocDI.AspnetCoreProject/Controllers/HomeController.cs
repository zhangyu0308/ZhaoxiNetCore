using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nest;
using Zhaoxi.Helper;
using Zhaoxi.IocDI.AspnetCoreProject.Models;
using Zhaoxi.IocDI.IBLL;
using Zhaoxi.IocDI.IDAL;
using Zhaoxi.IocDI.Model;

namespace Zhaoxi.IocDI.AspnetCoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IUserBLL _userBLL;

        public HomeController(ILogger<HomeController> logger, IUserBLL userBLL)
        {
            _logger = logger;
            this._userBLL = userBLL;
        }

        public IActionResult Index()
        {

            //新增
            //user user = new user();
            //user.Account = "555";
            //user.Age = 14;
            //user.Name = "zy";
            //this._userBLL.AddOnec(user);


            //全部查找
            var list = new List<FilterDefinition<user>>();
            list.Add(Builders<user>.Filter.Where(x => true));
            var filter = Builders<user>.Filter.And(list);
            var sort = Builders<user>.Sort.Descending("Age");
            var searchAllList = this._userBLL.FindListAsync(filter, sort);
            ViewBag.SearchUserList = searchAllList;

            //分页查找
            //var list = new List<FilterDefinition<user>>();
            //list.Add(Builders<user>.Filter.Where(x => true));
            //var filter = Builders<user>.Filter.And(list);
            //var sort = Builders<user>.Sort.Descending("Age");
            //var searchAllList = this._userBLL.FindListByPage(filter,1,2,null, sort);
            //ViewBag.SearchUserList = searchAllList;


            //条件查找
            //var wherelist = new List<FilterDefinition<user>>();
            //wherelist.Add(Builders<user>.Filter.Eq("Account", "rrr"));
            //var wherefilter = Builders<user>.Filter.And(wherelist);
            //var searchWhereList = this._userBLL.FindListAsync(wherefilter, sort);
            //ViewBag.SearchUserList = searchWhereList;

            //删除
            //this._userBLL.Delete(searchWhereList.FirstOrDefault().Account);

            //修改，先写修改的条件，然后是修改的字段
            //var firstmodel = searchAllList.FirstOrDefault();
            //firstmodel.Email = "123@qwe.com";
            //firstmodel.Password = "123456";
            //firstmodel.Role = "admin";
            //firstmodel.Age = 18;
            //this._userBLL.Edit(firstmodel.Account, firstmodel);

            //ES
            //var uris = new[] { new Uri("http://localhost:9200") };
            //var connectionPool = new SniffingConnectionPool(uris);
            //var settings = new ConnectionSettings(connectionPool).DefaultIndex("zhaoxi");
            //var client = new ElasticClient(settings);

            //创建客户端
            //var uris = new[] { new Uri("http://localhost:9200") };
            //var connectionPool = new SniffingConnectionPool(uris);
            //var settings = new ConnectionSettings(connectionPool);
            //var client = new ElasticClient(settings);

            //创建索引
            //CreateIndexResponse createIndexResponse = client.Indices.Create("zhaoxi1");

            //索引是否存在
            // var existsResponse = client.Indices.Exists("zhaoxi1");
            //if (existsResponse.Exists)
            //{
            //    DeleteIndexResponse deleteIndexResponse = client.Indices.Delete("zhaoxi1");
            //    var isdelete = deleteIndexResponse.Acknowledged;
            //}

            //user user = new user();
            //user.Account = "555";
            //user.Age = 14;
            //user.Name = "zy";
            //var indexResponse = client.IndexDocument(user);

            //            var searchResponse = client.Search<user>(s => s
            //    .From(0)
            //    .Size(10)
            //    .Query(q => q
            //         .Match(m => m
            //            .Field(f => f.Name)
            //            .Query("zy")
            //         )
            //    )
            //);
            //            var people = searchResponse.Documents;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
