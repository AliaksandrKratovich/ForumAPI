using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Forum.Dal.Repositories;
using Forum.Models.ArticlesManagement;
using Forum.Models.ErrorHandling;
using Forum.Services.ArticlesManagement;
using Forum.Tests.Helpers;
using Forum.WebApi.Controllers;
using Forum.WebApi.ErrorHandling;
using Moq;
using Xunit;

namespace Forum.Tests.Services
{
    public class ArticleServiceTests
    {
        private readonly Mock<IRepository<Article>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        public ArticleServiceTests()
        {
            _mockRepository = new Mock<IRepository<Article>>();
            _mockMapper = new Mock<IMapper>();
            _articleService = new ArticleService(_mockRepository.Object, _mockMapper.Object);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ArticleManagementMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CreateEntityAsyncTest_ReturnsArticleObject()
        {
            var articleRequest = ArticleHelper.CreateArticleRequest(null);
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(ArticleHelper.GetRandomTestArticles(10));
            var article = ArrangeFor_CreateEntityAsyncTest(articleRequest);

            var result = _articleService.CreateEntityAsync(articleRequest).Result;

            Assert.Equal(article.Category, result.Category);
            Assert.Equal(article.UserName, result.UserName);
            Assert.Equal(article.Content, result.Content);
            Assert.Equal(article.CreatedDate, result.CreatedDate);
            Assert.Equal(article.Id, result.Id);
            Assert.Equal(article.Title, result.Title);
        }

        [Fact]
        public void CreateEntityAsyncTest_WithExistingArticle_ReturnsBadRequestException()
        {
            var articleRequest = ArticleHelper.CreateArticleRequest(null);
            var article = ArrangeFor_CreateEntityAsyncTest(articleRequest);
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(new List<Article> { article });

            var exception = Assert.ThrowsAsync<BadRequestException>(() => _articleService.CreateEntityAsync(articleRequest));
            Assert.Equal("article already exists with such title", exception.Result.Message);
        }

        [Fact]
        public void CreateEntityAsyncTest_WithContentMoreThan2000Symbols_ReturnsBadRequestException()
        {
            var articleRequest = ArticleHelper.CreateArticleRequest(null);
            var article = ArrangeFor_CreateEntityAsyncTest(articleRequest);
            _mockRepository.Setup(r => r.GetAll()).ReturnsAsync(new List<Article> { article });
            articleRequest.Content = new string('*', 2500);

            var exception = Assert.ThrowsAsync<BadRequestException>(() => _articleService.CreateEntityAsync(articleRequest));
            Assert.Equal("articles bad content length", exception.Result.Message);
        }
        private Article ArrangeFor_CreateEntityAsyncTest(ArticleRequest articleRequest)
        {

            var article = _mapper.Map<ArticleRequest, Article>(articleRequest);
            _mockMapper.Setup(m => m.Map<ArticleRequest, Article>(articleRequest))
                .Returns(article);
            article.Id = Guid.NewGuid();

            _mockRepository.Setup(r => r.Add(article));

            return article;
        }

        [Fact]
        public void DeleteEntityAsyncTest_WithArticleId_ReturnsBooleanValue()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();
            _mockRepository.Setup(rep => rep.GetAll()).ReturnsAsync(new List<Article> { article });
            _mockRepository.Setup(rep => rep.Remove(article.Id)).ReturnsAsync(true);

            var result = _articleService.DeleteEntityAsync(article.Id).Result;

            Assert.True(result);
        }

        [Fact]
        public void DeleteEntityAsyncTest_WithNoneExistentId_ReturnsNotFoundException()
        {
            _mockRepository.Setup(rep => rep.GetAll()).ReturnsAsync(ArticleHelper.GetRandomTestArticles(10));
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();

            var exception = Assert.ThrowsAsync<NotFoundException>(() => _articleService.DeleteEntityAsync(article.Id));
            Assert.Equal("there is no article with such id", exception.Result.Message);
        }

        [Fact]
        public void UpdateEntityAsyncTest_WithArticleIdAndArticleRequest_ReturnsUpdatedArticleObject()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();
            var articleRequest = ArticleHelper.CreateArticleRequest(article);
            _mockMapper.Setup(m => m.Map<ArticleRequest, Article>(articleRequest)).Returns(article);
            _mockRepository.Setup(rep => rep.Update(article)).ReturnsAsync(true);

            var result = _articleService.UpdateEntityAsync(article.Id, articleRequest).Result;

            Assert.Equal(article.Category, result.Category);
            Assert.Equal(article.UserName, result.UserName);
            Assert.Equal(article.Content, result.Content);
            Assert.Equal(article.CreatedDate, result.CreatedDate);
            Assert.Equal(article.Id, result.Id);
            Assert.Equal(article.Title, result.Title);
        }

        [Fact]
        public void UpdateEntityAsyncTest_WithArticleRequestContentLengthMoreThan2000_ReturnsBadRequestException()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();
            var articleRequest = ArticleHelper.CreateArticleRequest(article);
            articleRequest.Content = new string('*', 2453);
            _mockMapper.Setup(m => m.Map<ArticleRequest, Article>(articleRequest)).Returns(article);
            _mockRepository.Setup(rep => rep.Update(article)).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<BadRequestException>(() =>
                    _articleService.UpdateEntityAsync(article.Id, articleRequest));

            Assert.Equal("article has  more than 2000 symbols content length", exception.Result.Message);
        }

        [Fact]
        public void UpdateEntityAsyncTest_WhenArticleWasNotUpdated_ReturnsResponseException()
        {
            var article = ArticleHelper.GetRandomTestArticles(1).FirstOrDefault();
            var articleRequest = ArticleHelper.CreateArticleRequest(article);
            _mockMapper.Setup(m => m.Map<ArticleRequest, Article>(articleRequest)).Returns(article);
            _mockRepository.Setup(rep => rep.Update(article)).ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<ResponseException>(() =>
                _articleService.UpdateEntityAsync(article.Id, articleRequest));

            Assert.Equal("article did not update", exception.Result.Message);
        }

        [Theory]
        [InlineData("Car", null, null)]
        [InlineData(null, "Mods", null)]
        [InlineData(null, null, "Amusement")]
        [InlineData("Car", "Mods", "Amusement")]
        void GetArticlesByOccurrenceAsyncTest_ReturnsListOfFilteredArticles(string title, string userName, string category)
        {
            var articles = ArticleHelper.GetTestArticles();
            var article = new Article
            {
                UserName = "Mods",
                Title = "Car",
                Category = Categories.Amusement,
                Content = "Volvo",
                CreatedDate = DateTime.Now
            };

            _mockRepository.Setup(rep => rep.GetAll()).ReturnsAsync(articles);

            var result = _articleService.GetArticlesByOccurrenceAsync(title, userName, category).Result.FirstOrDefault();

            Assert.Equal(article.Category, result.Category);
            Assert.Equal(article.UserName, result.UserName);
            Assert.Equal(article.Content, result.Content);
            Assert.Equal(article.Title, result.Title);
        }

        [Fact]
        public void GetEntityAsyncTest_WithArticleId_ReturnsSpecificArticleObject()
        {
            var article = ArticleHelper.GetRandomTestArticles(10).FirstOrDefault();
            _mockRepository.Setup(rep => rep.Find(article.Id)).ReturnsAsync(article);
            
            var result = _articleService.GetEntityAsync(article.Id).Result;

            Assert.Equal(article.Category, result.Category);
            Assert.Equal(article.UserName, result.UserName);
            Assert.Equal(article.Content, result.Content);
            Assert.Equal(article.CreatedDate, result.CreatedDate);
            Assert.Equal(article.Id, result.Id);
            Assert.Equal(article.Title, result.Title);
        }

        [Fact]
        public void GetEntityAsyncTest_WithNonexistentIdArticle_ReturnsNoteFoundException()
        {
            _mockRepository.Setup(rep => rep.Find(Guid.NewGuid()));

            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _articleService.GetEntityAsync(Guid.NewGuid()));

            Assert.Equal("not found article", exception.Result.Message);

        }
    }
}

