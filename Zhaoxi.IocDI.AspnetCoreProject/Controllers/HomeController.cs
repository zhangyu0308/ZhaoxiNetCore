using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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
        private IUserDAL _userDAL;

        public HomeController(ILogger<HomeController> logger, IUserBLL userBLL, IUserDAL userDAL)
        {
            _logger = logger;
            this._userBLL = userBLL;
            this._userDAL = userDAL;
        }

        public async Task<IActionResult> Index()
        {
            //注入方式
            var model = this._userBLL.Login("123");


            //新增
            //user user = new user();
            //user.Account = "111";
            //user.Age = 120;
            //user.Name ="asdddd";
            //this._userBLL.AddOnec(user);

            //删除
            // this._userBLL.Delete("111");
            var user = this._userBLL.FindAll().FirstOrDefault();
            user.Name = "zhangyusheng";

            //修改方法不对
            //this._userBLL.Edit("111",user);

            //查找全部
            var list = this._userBLL.FindAll();
            ViewBag.UserList = list;
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
