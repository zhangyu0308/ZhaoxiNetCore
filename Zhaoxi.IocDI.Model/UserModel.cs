using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime LoginTime { get; set; }

    }
    [BsonIgnoreExtraElements]
    public class user
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverterExtensions))]
        public Object _id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [BsonDateTimeOptions]
        public DateTime LoginTime { get; set; } = DateTime.Now;
        public int Sex { get; set; }
    }
}
