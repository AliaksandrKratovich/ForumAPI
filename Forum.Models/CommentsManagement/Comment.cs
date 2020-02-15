using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Forum.Models.CommentsManagement
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ArticleId { get; set; }

        [BsonDateTimeOptions]
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }

        public string UserName { get; set; }
    }
}
