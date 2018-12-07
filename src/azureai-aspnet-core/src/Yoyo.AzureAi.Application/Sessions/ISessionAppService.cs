using System.Threading.Tasks;
using Abp.Application.Services;
using Yoyo.AzureAi.Sessions.Dto;

namespace Yoyo.AzureAi.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
