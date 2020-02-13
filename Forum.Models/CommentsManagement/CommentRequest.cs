using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Models.CommentsManagement
{
    public class CommentRequest
    {
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string Content { get; set; }
    }
}
