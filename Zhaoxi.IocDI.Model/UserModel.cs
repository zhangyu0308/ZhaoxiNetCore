using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    [Serializable]
    [BsonIgnoreExtraElements]
    public class user
    {
        [BsonId]
        //如果Id是string类型，就使用下边这个特性,//如果Id是ObjectId类型，就使用后边边这个特性 [JsonConverter(typeof(ObjectIdConverterExtensions))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
