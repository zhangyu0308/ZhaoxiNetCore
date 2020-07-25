using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetCoreWebApi.Utility;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentcationController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _iConfiguration;
        private JWTService _jWTService;
        public AuthentcationController(ILogger<WeatherForecastController> logger, IConfiguration iConfiguration, JWTService jWTService)
        {
            _logger = logger;
            _iConfiguration = iConfiguration;
            _jWTService = jWTService;
        }
        // GET: api/<AuthentcationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpPost]
        [Route("login")]
        [Authorize]
        public string Login(string name, string password)
        {
            if ("zy".Equals(name) && "123456".Equals(password))
            {
                string token = _jWTService.GetToken(name);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token=""
                });
            }
        }
    }
}
