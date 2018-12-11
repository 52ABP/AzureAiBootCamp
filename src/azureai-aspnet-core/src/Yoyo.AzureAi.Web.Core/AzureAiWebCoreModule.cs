using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Yoyo.AzureAi.Authentication.JwtBearer;
using Yoyo.AzureAi.Configuration;
using Yoyo.AzureAi.EntityFrameworkCore;
using Abp.Configuration.Startup;
using Newtonsoft.Json;

namespace Yoyo.AzureAi
{
    [DependsOn(
         typeof(AzureAiApplicationModule),
         typeof(AzureAiEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        , typeof(AbpAspNetCoreSignalRModule)
     )]
    public class AzureAiWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IConfigurationRoot _azureConfiguration;

        public AzureAiWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            _azureConfiguration = env.GetAzureConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AzureAiConsts.ConnectionStringName
            );

            // TODO:将所有错误信息显示到客户端
            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(AzureAiApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            #region TokenAuthConfiguration

            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);

            #endregion


            #region AzureConfiguration

            IocManager.Register<AzureConfigs>();
            var azureConfigs = IocManager.Resolve<AzureConfigs>();
            azureConfigs = JsonConvert.DeserializeObject<AzureConfigs>(_azureConfiguration["AzureCognitiveServices"].ToString());
            #endregion
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AzureAiWebCoreModule).GetAssembly());
        }
    }
}
