using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Yoyo.AzureAi.Authorization;
using Yoyo.AzureAi.Authorization.Roles;
using Yoyo.AzureAi.Authorization.Users;
using Yoyo.AzureAi.Editions;
using Yoyo.AzureAi.MultiTenancy;

namespace Yoyo.AzureAi.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>()
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpLogInManager<LogInManager>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
