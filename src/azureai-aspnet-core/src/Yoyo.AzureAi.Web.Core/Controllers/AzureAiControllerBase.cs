using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Yoyo.AzureAi.Controllers
{
    public abstract class AzureAiControllerBase: AbpController
    {
        protected AzureAiControllerBase()
        {
            LocalizationSourceName = AzureAiConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
