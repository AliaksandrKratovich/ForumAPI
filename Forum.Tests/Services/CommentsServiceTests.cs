using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Forum.Dal.Repositories;
using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Forum.Models.ErrorHandling;
using Forum.Services.CommentsManagement;
using Forum.Tests.Helpers;
using Forum.WebApi.ErrorHandling;
using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using Xunit;

namespace Forum.Tests.Services
{
    public class CommentsServiceTests
    {
        private readonly Mock<IRepository<Comment>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public CommentsServiceTests()
        {
            _mockRepository = new Mock<IRepository<Comment>>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentService(_mockRepository.Object, _mockMapper.Object);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CommentManagementMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CreateEntityAsyncTest_WithCommentRequest_ReturnsCommentObject()
        {
            var commentRequest = CommentHelper.CreateCommentRequest(null);
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            _mockRepository.Setup(repo => repo.Add(comment));
            _mockMapper.Setup(m => m.Map<CommentRequest, Comment>(commentRequest)).Returns(comment);

            var result = _commentService.CreateEntityAsync(commentRequest).Result;

            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.ArticleId, result.ArticleId);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.UserName, result.UserName);
            Assert.Equal(comment.CreatedDate, result.CreatedDate);
        }

        [Fact]
        public void CreateEntityAsyncTest_WithCommentRequestWithContentLengthMoreThan200Symbols_ReturnsBadRequestException()
        {
            var commentRequest = CommentHelper.CreateCommentRequest(null);
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            _mockRepository.Setup(repo => repo.Add(comment));
            _mockMapper.Setup(m => m.Map<CommentRequest, Comment>(commentRequest)).Returns(comment);
            commentRequest.Content = new string('$', 352);

            var exception = Assert.ThrowsAsync<BadRequestException>(() => _commentService.CreateEntityAsync(commentRequest));

            Assert.Equal("comments bad content length", exception.Result.Message);
        }

        [Fact]
        public void DeleteEntityAsyncTest_WithCommentId_ReturnsBooleanValue()
        {
            var comments = CommentHelper.GetRandomTestComments(10);
            var commentId = comments.First().Id;
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(comments);
            _mockRepository.Setup(m => m.Remove(commentId)).ReturnsAsync(true);

            var result = _commentService.DeleteEntityAsync(commentId).Result;

            Assert.True(result);
        }

        [Fact]
        public void DeleteEntityAsyncTest_WithNonexistentCommentId_ReturnsNotFoundException()
        {
            var commentId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(CommentHelper.GetRandomTestComments(10));
            _mockRepository.Setup(m => m.Remove(commentId)).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<NotFoundException>(() => _commentService.DeleteEntityAsync(commentId));

            Assert.Equal("there is no comment with such id", exception.Result.Message);
        }

        [Fact]
        public void GetCommentsByArticleIdTest_WithArticleId_ReturnsListOfCommentsObjects()
        {
            var comments = CommentHelper.GetRandomTestComments(10);
            var articleId = Guid.NewGuid();
            for (int i = 0; i < comments.Count / 2; i++)
            {
                comments[i].ArticleId = articleId;
            }
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(comments);
            var expected = comments.GetRange(0, comments.Count / 2);

            var result = _commentService.GetCommentsByArticleId(articleId).Result;

            Assert.Contains(expected, comment => result.Any(a =>
                                                                 a.UserName == comment.UserName &&
                                                                 a.Content == comment.Content &&
                                                                 a.CreatedDate == comment.CreatedDate &&
                                                                 a.Id == comment.Id &&
                                                                 a.ArticleId == comment.ArticleId
            ));

        }

        [Fact]
        public void GetEntityAsyncTest_WithCommentId_ReturnsCommentObject()
        {
            var comments = CommentHelper.GetRandomTestComments(10);
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(comments);

            var result = _commentService.GetEntitiesAsync().Result;
            Assert.Contains(comments, comment => result.Any(a =>
                a.UserName == comment.UserName &&
                a.Content == comment.Content &&
                a.CreatedDate == comment.CreatedDate &&
                a.Id == comment.Id &&
                a.ArticleId == comment.ArticleId
            ));
        }

        [Fact]
        public void GetEntityAsyncTest_WithNonexistentCommentId_ReturnsNotFoundException()
        {
            var comment = CommentHelper.GetRandomTestComments(1).First();
            _mockRepository.Setup(repo => repo.Find(comment.Id)).ReturnsAsync(comment);

            var result = _commentService.GetEntityAsync(comment.Id).Result;

            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.ArticleId, result.ArticleId);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.UserName, result.UserName);
            Assert.Equal(comment.CreatedDate, result.CreatedDate);
        }

        [Fact]
        public void GetEntityAsync_WithNonexistentCommentId_ReturnsNotFoundException()
        {
            var comment = CommentHelper.GetRandomTestComments(1).First();
            _mockRepository.Setup(repo => repo.Find(comment.Id));

            var exception = Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetEntityAsync(comment.Id));

            Assert.Equal("not found comment", exception.Result.Message);
        }

        [Fact]
        public void UpdateEntityAsyncTest_WithCommentIdAndCommentRequest_ReturnsCommentObject()
        {
            var commentRequest = CommentHelper.CreateCommentRequest(null);
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            _mockRepository.Setup(repo => repo.Update(comment)).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<CommentRequest, Comment>(commentRequest)).Returns(comment);

            var result = _commentService.UpdateEntityAsync(comment.Id, commentRequest).Result;

            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.ArticleId, result.ArticleId);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.UserName, result.UserName);
            Assert.Equal(comment.CreatedDate, result.CreatedDate);
        }


        [Fact]
        public void UpdateEntityAsyncTest_WithCommentIdAndCommentRequestWhenCommentWasNotUpdated_ReturnsResponseException()
        {
            var commentRequest = CommentHelper.CreateCommentRequest(null);
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            _mockRepository.Setup(repo => repo.Update(comment)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<CommentRequest, Comment>(commentRequest)).Returns(comment);

            var exception = Assert.ThrowsAsync<ResponseException>(() => _commentService.UpdateEntityAsync(comment.Id, commentRequest));

            Assert.Equal("comment did not update", exception.Result.Message);
        }

        [Fact]
        public void UpdateEntityAsyncTest_WithCommentIdAndCommentRequestWithContentLengthMoreThan200Symbols_ReturnsBadRequestException()
        {
            var commentRequest = CommentHelper.CreateCommentRequest(null);
            commentRequest.Content = new string('4', 234);
            var comment = _mapper.Map<CommentRequest, Comment>(commentRequest);
            _mockRepository.Setup(repo => repo.Update(comment)).ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<BadRequestException>(() => _commentService.UpdateEntityAsync(comment.Id, commentRequest));

            Assert.Equal("comment has more than 200 symbols content length", exception.Result.Message);
        }
    }
}
