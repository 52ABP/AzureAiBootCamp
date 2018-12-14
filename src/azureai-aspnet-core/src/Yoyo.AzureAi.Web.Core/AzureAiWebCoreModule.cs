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
using Abp.AutoMapper;

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

        public AzureAiWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AzureAiConsts.ConnectionStringName
            );

#if DEBUG
            // TODO:将所有错误信息显示到客户端
            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;
#endif


            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(AzureAiApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();

            ConfigureAzure();
        }

        private void ConfigureTokenAuth()
        {

            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);

        }

        private void ConfigureAzure()
        {
            // 注入,然后实例化
            IocManager.Register<AzureConfigs>();
            var azureConfigs = IocManager.Resolve<AzureConfigs>();

            // 读取配置文件并反序列化
            var auzreSettingsContent = _env.GetJsonFileContent("azuresettings");
            var tmpAzureConfigs = JsonConvert.DeserializeObject<AzureConfigs>(auzreSettingsContent);

            // 将临时配置对象中的引用给Ioc实例化对象
            azureConfigs.ComputerVisionOCR = tmpAzureConfigs.ComputerVisionOCR;
            azureConfigs.ComputerVisionImgAnalyze = tmpAzureConfigs.ComputerVisionImgAnalyze;
            azureConfigs.SpeechToText = tmpAzureConfigs.SpeechToText;
            azureConfigs.TextToSpeech = tmpAzureConfigs.TextToSpeech;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AzureAiWebCoreModule).GetAssembly());
        }
    }
}
