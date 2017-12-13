using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Colipu.Tools.Model.LogModel
{
    public class LogInfo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId _id { set; get; }

        /// <summary>
        /// APPID
        /// </summary>
        [BsonElement("appid")]
        public string APPID { set; get; }

        /// <summary>
        /// 等级
        /// </summary>
        [BsonElement("level")]
        public string Level { set; get; }

        /// <summary>
        /// 日志所在方法
        /// </summary>
        [BsonElement("method")]
        public string Method { set; get; }

        /// <summary>
        /// Title
        /// </summary>
        [BsonElement("title")]
        public string Title { set; get; }

        /// <summary>
        /// Index
        /// </summary>
        [BsonElement("index")]
        public string Index { set; get; }

        /// <summary>
        /// Index
        /// </summary>
        [BsonElement("group")]
        public string Group { set; get; }

        /// <summary>
        /// ClientMessage
        /// </summary>
        [BsonElement("message")]
        public string Message { set; get; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [BsonElement("exception")]
        public string Exception { set; get; }

        /// <summary>
        /// ServerIP
        /// </summary>
        [BsonElement("serverip")]
        public string ServerIP { set; get; }

        /// <summary>
        /// ServerIP
        /// </summary>
        [BsonElement("logdate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LogDate { set; get; }
    }
}
