using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Giovanni.Services.Database.MongoDB
{
    public class HashTableDocument
    {
        public ObjectId Id { get; set; }

        [BsonExtraElements]
        public Dictionary<string, object> Values { get; set; }
    }
}