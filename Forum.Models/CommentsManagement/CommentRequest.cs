﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Models.CommentsManagement
{
    public class CommentRequest
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

    }
}
