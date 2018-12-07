using Abp.AutoMapper;
using Yoyo.AzureAi.Authentication.External;

namespace Yoyo.AzureAi.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
