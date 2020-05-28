using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    public interface ICommonModel<TKey>
    {
        [BsonId]
        string Id { get; set; }

        [JsonIgnore]
        ObjectId ObjectId { get; }
    }

    [BsonIgnoreExtraElements(Inherited = true)]
    public class CommonModel<TKey> : ICommonModel<TKey>
    {
        public CommonModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
        [JsonProperty(Order = 1)]
        [BsonElement(Order = 0)]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [JsonIgnore]
        public ObjectId ObjectId
        {
            get
            {
                if (Id == null)
                    Id = ObjectId.GenerateNewId().ToString();
                return ObjectId.Parse(Id);
            }
        }
    }
    public class CommonModel : CommonModel<string>
    {

    }
}
