using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Timing;

namespace XUCore.NetCore.Mongo
{
    [Serializable]
    [Mongo]
    public class MongoEntity
    {
        /// <summary>
        /// 自定义标识的主键ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        /// <summary>
        /// 记录时间戳
        /// </summary>
        [BsonElement("RecordTicks")]
        [BsonRepresentation(BsonType.Int64)]
        public long RecordTicks { get; set; } = DateTime.Now.ToTimeStamp(true);
    }
}
