using Forum.Dal.Repositories;
using Forum.Dal.Repositories.UserRepository;
using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Forum.Services.ArticlesManagement;
using Forum.Services.CommentsManagement;
using Forum.Services.UserManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.WebApi.Configurations
{
    public class DIConfiguration : IApplicationConfiguration
    {
        public void InstallConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<Article>, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
