using System;
using System.Collections.Generic;
using System.Text;
using Forum.Models.CommentsManagement;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Forum.Tests.Helpers
{
    public class CommentHelper
    {
        private static Random _random = new Random();

        public static List<Comment> GetRandomTestComments(int amount)
        {
            var commentsList = new List<Comment>();
            for (int i = 0; i < amount; i++)
            {
                commentsList.Add(new Comment
                {
                    Id = Guid.NewGuid(),
                    ArticleId = Guid.NewGuid(),
                    UserName = GeneralHelpers.GetRandomString(_random.Next(10) + 1),
                    Content = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    CreatedDate = DateTime.Now
                });
            }

            return commentsList;
        }

        public static CommentRequest CreateCommentRequest(Comment comment)
        {
            if (comment == null)
            {
                return new CommentRequest
                {
                    Id = Guid.NewGuid(),
                    ArticleId = Guid.NewGuid(),
                    UserName = GeneralHelpers.GetRandomString(_random.Next(10) + 1),
                    Content = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                };
            }
            return new CommentRequest
            {
                Id = comment.Id,
                ArticleId = comment.ArticleId,
                UserName = comment.UserName,
                Content = comment.Content
            };
        }
    }
}
