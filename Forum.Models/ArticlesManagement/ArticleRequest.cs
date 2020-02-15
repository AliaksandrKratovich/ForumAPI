using System;

namespace Forum.Models.ArticlesManagement
{
    public class ArticleRequest
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; }
        public string UserName { get; set; }

    }
}
