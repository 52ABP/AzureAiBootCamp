using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.AzureAi.MultiTenancy.Dto;

namespace Yoyo.AzureAi.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
