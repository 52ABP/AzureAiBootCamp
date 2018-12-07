using System.Threading.Tasks;
using Yoyo.AzureAi.Configuration.Dto;

namespace Yoyo.AzureAi.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
