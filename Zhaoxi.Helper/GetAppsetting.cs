using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zhaoxi.Helper
{
    public class GetAppsetting
    {
        //需要导入Microsoft.AspNetCore包
        public static IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        public static IConfigurationSection GetAppsettingName(string appsetttingname= "MongoDB")
        {
            return configuration.GetSection(appsetttingname);
        }


        /// <summary>
        /// 根据Key取Value值   appsettings.json 的值
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppsettingsValue(string keyname,string appsetttingname = "MongoDB")
        {
            try
            {
                return GetAppsettingName(appsetttingname).GetSection(keyname).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
