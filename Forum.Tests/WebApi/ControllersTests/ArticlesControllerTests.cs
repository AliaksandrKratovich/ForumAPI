using Forum.Models.ArticlesManagement;
using Forum.Services.ArticlesManagement;
using Forum.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Forum.Tests.Helpers;
using Xunit;

namespace Forum.Tests.WebApi.ControllersTests
{
    public class ArticlesControllerTests
    {
        private readonly Mock<IArticleService> _mockArticleService;
        private readonly ArticlesController _controller;
        private readonly Random _random;
        public ArticlesControllerTests()
        {
            _mockArticleService = new Mock<IArticleService>();
            _controller = new ArticlesController(_mockArticleService.Object);
            _random = new Random();
        }
        [Fact]
        public void GetArticlesTest_ReturnsOkResultWithListOfArticles()
        {
            var articles = ArticleHelper.GetRandomTestArticles(10);
            _mockArticleService.Setup(ser => ser.GetEntitiesAsync()).ReturnsAsync(articles);

            var okObjectResult = (_controller.GetArticles().Result.Result as OkObjectResult)?.Value;
            var result = okObjectResult as IEnumerable<Article> ?? throw new InvalidOperationException();

            Assert.Contains(result, article => articles.Any(a => a.Category == article.Category &&
                                                                 a.UserName == article.UserName &&
                                                                 a.Content == article.Content &&
                                                                 a.CreatedDate == article.CreatedDate &&
                                                                 a.Id == article.Id && 
                                                                 a.Title == article.Title
                                                                 ));
        }

        

        [Fact]
        public void GetSortedArticlesTest_ReturnsListOfArticles()
        {
            var articles = ArticleHelper.GetRandomTestArticles(10);
            var expectedArticles = articles.Take(3).ToList();

            string title = expectedArticles.ElementAt(0).Title;
            string userName = expectedArticles.ElementAt(1).UserName;
            string category = expectedArticles.ElementAt(2).Category.ToString();

            _mockArticleService.Setup(ser => ser.GetArticlesByOccurrenceAsync(title, userName, category)
            ).ReturnsAsync(expectedArticles);

            var okObjectResult = (_controller.GetSortedArticles( title, userName,category).Result.Result as OkObjectResult)?.Value;

            var result = okObjectResult as IEnumerable<Article> ?? throw new InvalidOperationException();

            Assert.Contains(result, article => articles.Any(a => a.Category == article.Category &&
                                                                 a.UserName == article.UserName &&
                                                                 a.Content == article.Content &&
                                                                 a.CreatedDate == article.CreatedDate &&
                                                                 a.Id == article.Id &&
                                                                 a.Title == article.Title));
        }

        [Fact]
        public void GetArticleTest_ReturnsFoundArticle()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();
            _mockArticleService.Setup(serv => serv.GetEntityAsync(article.Id)).ReturnsAsync(article);

            var resultArticle = (_controller.GetArticle(article.Id).Result.Result as OkObjectResult)?.Value as Article;
            
            Assert.Equal(article.Category, resultArticle.Category);
            Assert.Equal( article.UserName, resultArticle.UserName);
            Assert.Equal(article.Content, resultArticle.Content);
            Assert.Equal(article.CreatedDate, resultArticle.CreatedDate);
            Assert.Equal(article.Id, resultArticle.Id);
            Assert.Equal(article.Title, resultArticle.Title);
        }

        [Fact]
        public void AddArticleTest_ReturnsCreatedArticle()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();

            ClaimsIdentity c = new ClaimsIdentity(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, article.UserName)
            }));
            ClaimsPrincipal cp = new ClaimsPrincipal(c);
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.Setup(c => c.User).Returns(cp);
            var articleRequest = ArticleHelper.CreateArticleRequest(article);
            _mockArticleService.Setup(serv => serv.CreateEntityAsync(articleRequest)).ReturnsAsync(article);
            _controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultArticle = (_controller.AddArticle(articleRequest).Result.Result as CreatedResult)?.Value as Article;

            Assert.Equal(article.Category, resultArticle.Category);
            Assert.Equal(article.UserName, resultArticle.UserName);
            Assert.Equal(article.Content, resultArticle.Content);
            Assert.Equal(article.CreatedDate, resultArticle.CreatedDate);
            Assert.Equal(article.Id, resultArticle.Id);
            Assert.Equal(article.Title, resultArticle.Title);
        }

    }
}
