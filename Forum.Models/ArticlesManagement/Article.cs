using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Forum.Models.ArticlesManagement
{
    public class Article
    {
        [BsonId]
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Categories Category { get; set; } = Categories.None;
        public string Content { get; set; } = string.Empty;
        [BsonDateTimeOptions]
        public DateTime CreatedDate { get; set; }
        public IEnumerable<Guid> CommentsId { get; set; }
    }
}
