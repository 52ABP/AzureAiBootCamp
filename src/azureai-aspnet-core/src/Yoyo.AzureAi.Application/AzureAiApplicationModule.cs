using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Yoyo.AzureAi.Authorization;

namespace Yoyo.AzureAi
{
    [DependsOn(
        typeof(AzureAiCoreModule),
        typeof(AbpAutoMapperModule))]
    public class AzureAiApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AzureAiAuthorizationProvider>();

            // 自定义类型映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                // XXXMapper.CreateMappers(configuration);


            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AzureAiApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
