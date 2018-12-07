using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.AzureAi.Configuration;

namespace Yoyo.AzureAi.Web.Host.Startup
{
    [DependsOn(
       typeof(AzureAiWebCoreModule))]
    public class AzureAiWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AzureAiWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AzureAiWebHostModule).GetAssembly());
        }
    }
}
