using System.Threading.Tasks;
using Abp.Application.Services;
using Yoyo.AzureAi.Authorization.Accounts.Dto;

namespace Yoyo.AzureAi.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
