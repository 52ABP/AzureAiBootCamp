using Abp.Authorization;
using Yoyo.AzureAi.Authorization.Roles;
using Yoyo.AzureAi.Authorization.Users;

namespace Yoyo.AzureAi.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
