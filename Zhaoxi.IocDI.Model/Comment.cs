using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    [BsonIgnoreExtraElements]
    public class Comment
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverterExtensions))]
        public string Id { get; set; }
        public string Content { get; set; }
        [BsonDateTimeOptions]
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 索引字段
        /// </summary>
        public string UserId { get; set; }
        public string NickName { get; set; }

        [BsonDateTimeOptions]
        public DateTime CreateDateTime { get; set; }
        public int LikeNum { get; set; }
        public int ReplyNum { get; set; }
        public string State { get; set; }
        public string ParentId { get; set; }
    }
}
