using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HcCrm.Util
{
    public class Config
    {
        public static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        /// <summary>
        /// 获取AppSettings的Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            try
            {
                var tempvalue = configuration.GetSection("AppSettings").GetSection(key).Value;
                return string.IsNullOrEmpty(tempvalue) ? "" : tempvalue.Trim();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 根据Key取Value值 configuration.GetSection("Logging:LogLevel:Default")
        /// </summary>
        /// <param name="key"></param>
        public static string GetJsonValue(string keypath)
        {
            try
            {

                return configuration.GetSection(keypath).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 根据Key取Value值 configuration.GetSection("Logging:LogLevel:Default")
        /// </summary>
        /// <param name="key"></param>
        public static string GetJsonConnValue(string dbtypename)
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("appsettings.json")
                                   .Build();

                return configuration.GetConnectionString(dbtypename);
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

    }
}
