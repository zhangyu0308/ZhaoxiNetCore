using HcCrm.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.Helper
{
    //根据配置文件切换数据库
    public class DbFactory
    {
        private readonly static string dbtype = Config.GetValue("DBServerType");

    }
    public enum DatabaseType
    {
        /// <summary>
        /// 数据库类型：SqlServer
        /// </summary>
        SqlServer,
        /// <summary>
        /// 数据库类型：MySql
        /// </summary>
        MySql
    }
}
