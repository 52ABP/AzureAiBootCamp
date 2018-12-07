using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace Yoyo.AzureAi.Authorization.Accounts.Dto
{
    public class IsTenantAvailableInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}
