using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Yoyo.AzureAi.Authorization.Roles;
using Yoyo.AzureAi.Authorization.Users;
using Yoyo.AzureAi.MultiTenancy;

namespace Yoyo.AzureAi.EntityFrameworkCore
{
    public class AzureAiDbContext : AbpZeroDbContext<Tenant, Role, User, AzureAiDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public AzureAiDbContext(DbContextOptions<AzureAiDbContext> options)
            : base(options)
        {
        }
    }
}
