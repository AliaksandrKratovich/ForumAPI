using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Forum.Models.ArticlesManagement
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Categories Category { get; set; } = Categories.None;
        public string Content { get; set; } = string.Empty;
        [BsonDateTimeOptions]
        public DateTime CreatedDate { get; set; }


    }
}
