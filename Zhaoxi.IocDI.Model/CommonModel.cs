using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    public class CommonModel 
    {
        public string Id { get; set; }
        [BsonDateTimeOptions]
        public DateTime CreateTime { get; set; }
    }
}
