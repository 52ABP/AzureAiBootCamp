using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Yoyo.AzureAi.Configuration
{
    public static class HostingEnvironmentExtensions
    {
        public static IConfigurationRoot GetAppConfiguration(this IHostingEnvironment env)
        {
            return AppConfigurations.GetAppsettings(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }

        public static IConfigurationRoot GetAzureConfiguration(this IHostingEnvironment env)
        {
            return AppConfigurations.GetAzuresettings(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }
    }
}
