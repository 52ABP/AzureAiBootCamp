using Microsoft.AspNetCore.Antiforgery;
using Yoyo.AzureAi.Controllers;

namespace Yoyo.AzureAi.Web.Host.Controllers
{
    public class AntiForgeryController : AzureAiControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
