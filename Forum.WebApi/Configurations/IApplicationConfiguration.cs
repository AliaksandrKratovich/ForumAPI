using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.WebApi.Configurations
{
    public interface IApplicationConfiguration
    {
        void InstallConfigurations(IServiceCollection services, IConfiguration configuration);
    }
}
