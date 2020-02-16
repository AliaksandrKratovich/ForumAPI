using Forum.Dal;
using Forum.Dal.DatabaseAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.WebApi.Configurations
{
    public class DatabaseConfiguration : IApplicationConfiguration
    {
        public void InstallConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Settings>(options =>
            {
                options.ConnectionString
                    = configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = configuration.GetSection("MongoConnection:Database").Value;
                options.CommentsCollection
                    = configuration.GetSection("MongoConnection:CommentsCollection").Value;
                options.ArticleCollection
                    = configuration.GetSection("MongoConnection:ArticleCollection").Value;
                options.UserCollection
                    = configuration.GetSection("MongoConnection:UserCollection").Value;
            });

            services.AddSingleton<ApplicationContext>();
        }
    }
}
