using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Forum.Models.CommentsManagement
{
   public class Comment
    {
        [BsonId]
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        [BsonDateTimeOptions]
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
    }
}
