using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yoyo.AzureAi.Controllers;
using Yoyo.AzureAi.Web.Host.AI.ComputerVisionImgAnalyze;
using Yoyo.AzureAi.Web.Host.AI.ComputerVisionOCR;

namespace Yoyo.AzureAi.Web.Host.Controllers
{
    public class AIController : AzureAiControllerBase
    {
        public IActionResult ComputerVisionImgAnalyze()
        {
            return View();
        }

        public IActionResult ComputerVisionOCR()
        {
            return View();
        }

        public async Task<IActionResult> JudgeComputerVisionOCR(string url)
        {
            var r=await ComputerVisionOCRDemo.Run(url);
            return Json(r);
        }

        public async Task<IActionResult> JudgeComputerVisionImgAnalyze(string url)
        {
            var r = await ComputerVisionImgAnalyzeDemo.Run(url);
            return Json(r);
        }
    }
}