using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zhaoxi.IocDI.AspnetCoreProject.Models;
using Zhaoxi.IocDI.IBLL;
using Zhaoxi.IocDI.IDAL;

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

        public IActionResult Index()
        {
            var model = this._userBLL.Login("123");
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
