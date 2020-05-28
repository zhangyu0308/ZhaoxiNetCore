using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    public class UserModel: CommonModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime LoginTime { get; set; }

    }
    public class user : CommonModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime LoginTime { get; set; }

    }
}
