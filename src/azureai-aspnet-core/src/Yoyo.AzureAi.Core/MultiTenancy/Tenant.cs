using Abp.MultiTenancy;
using Yoyo.AzureAi.Authorization.Users;

namespace Yoyo.AzureAi.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
