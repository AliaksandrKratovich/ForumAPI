using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Models.ArticlesManagement
{
    public class ArticleRequest
    {
        public string Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; }
        public string UserName { get; set; }

    }
}
