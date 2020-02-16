using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Forum.Models.ArticlesManagement;

namespace Forum.Tests.Helpers
{
   public static class ArticleHelper
    {
        private static Random _random = new Random();
        public static List<Article> GetRandomTestArticles(int amount)
        {
            var articlesList = new List<Article>();
            for (int i = 0; i < amount; i++)
            {
                articlesList.Add(new Article
                {
                    Id = Guid.NewGuid(),
                    UserName = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    Title = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    Category = (Categories)_random.Next(5),
                    Content = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    CreatedDate = DateTime.Now
                });
            }

            return articlesList;
        }
      
        public static ArticleRequest CreateArticleRequest(Article article)
        {
            if (article == null)
            {
                return new ArticleRequest
                {
                    Id = Guid.NewGuid(),
                    UserName = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    Title = GeneralHelpers.GetRandomString(_random.Next(15) + 1),
                    Category = ((Categories)_random.Next(5)).ToString(),
                    Content = GeneralHelpers.GetRandomString(_random.Next(15) + 1)
                };
            }
            return new ArticleRequest
            {
                Id = article.Id,
                UserName = article.UserName,
                Title = article.Title,
                Category = article.Category.ToString(),
                Content = article.Content
            };
        }

        public static List<Article> GetTestArticles()
        {
            var articlesList = new List<Article>();
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Alex",
                Title = "Some interesting",
                Category = Categories.Develop,
                Content = "qwert",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Mods",
                Title = "Car",
                Category = Categories.Amusement,
                Content = "Volvo",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Joe",
                Title = "Phones",
                Category = Categories.None,
                Content = "Dora phone",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Gura",
                Title = "Circling",
                Category = Categories.Sport,
                Content = "FAr away",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Rom",
                Title = "Greater way",
                Category = Categories.Business,
                Content = "Open IB",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Doas",
                Title = "Senior way",
                Category = Categories.Develop,
                Content = ".net",
                CreatedDate = DateTime.Now
            });
            articlesList.Add(new Article
            {
                Id = Guid.NewGuid(),
                UserName = "Mustafa",
                Title = "bred kakoy to",
                Category = Categories.None,
                Content = "qwesgrsegwesgrwrt",
                CreatedDate = DateTime.Now
            });
            return articlesList;
        }
    }
}
