using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.IocDI.Model
{
    /// <summary>
    /// 解决：ObjectId对象无法正常序列化字符串对象
    /// C#Driver将mongodb数据读取到类型对象上的时候，转换json对象过程出现了objectid字段无法正常转换为字符串的现象（取而代之的是objectid的每个字符+换行符）
    /// </summary>
    public class ObjectIdConverterExtensions : JsonConverter
    {
        //重写序列化
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing ObjectId. Expected String.get{reader.TokenType}");
            }
            var value = (string)reader.Value;
            return string.IsNullOrEmpty(value) ? ObjectId.Empty : ObjectId.Parse(value);
        }
        //重新反序列化
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
        }
    }
}
