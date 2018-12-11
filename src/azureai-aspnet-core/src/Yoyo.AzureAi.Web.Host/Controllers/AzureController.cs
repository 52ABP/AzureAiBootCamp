using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yoyo.AzureAi.AzureCognitive;
using Yoyo.AzureAi.Controllers;


namespace Yoyo.AzureAi.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AzureController : AzureAiControllerBase
    {
        private readonly IAzureCognitiveManager _azureCognitiveManager;
        public AzureController(IAzureCognitiveManager azureCognitiveManager)
        {
            _azureCognitiveManager = azureCognitiveManager;
        }

        [HttpPost]
        public async Task<string> Ocr(string imgUrl, string language)
        {
            var result = await _azureCognitiveManager.ImgOcrSimpleFormattedText(imgUrl, language);

            return result;

        }

    }
}