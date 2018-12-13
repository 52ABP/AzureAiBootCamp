using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Yoyo.AzureAi.AzureCognitive;
using Yoyo.AzureAi.AzureCognitive.Dtos;
using Yoyo.AzureAi.Controllers;
using Yoyo.AzureAi.Net.MimeTypes;

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

        /// <summary>
        /// 图片场景分析
        /// </summary>
        /// <param name="imgUrl">图片链接</param>
        /// <returns>分析结果Dto</returns>
        [HttpPost]
        [DisableAuditing]
        public async Task<ImgSceneRecognitionDto> ImgSceneRecognition([FromBody]ImgSceneRecognitionInput input)
        {
            var analyzeResult = await _azureCognitiveManager.ImgAnalyze(input.ImgUrl);


            // 主色彩
            //analyzeResult.Color.DominantColors

            // 是否黑白照
            // analyzeResult.Color.IsBWImg


            var result = new ImgSceneRecognitionDto()
            {
                Captions = GetCaptions(analyzeResult),
                ImgTags = GetImgTag(analyzeResult),
                Faces = analyzeResult.Faces?.ToList()// 图片中的人脸和相关信息
            };

            return result;
        }


        /// <summary>
        /// 图片Ocr
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>ocr识别结果</returns>
        [HttpPost]
        [DisableAuditing]
        public async Task<string> ImgOcr([FromBody]ImgOcrInput input)
        {
            var result = await _azureCognitiveManager.ImgOcrSimpleFormattedText(input.ImgUrl, input.Lang);

            return result;

        }

        /// <summary>
        /// 文字转语音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableAuditing]
        public async Task<FileContentResult> TextToSpeech([FromBody]TextToSpeechInput input)
        {
            try
            {
                var result = await _azureCognitiveManager.TextToSpeech(input.Text, input.Lang, input.Voice);

                return File(result, MimeTypeNames.ApplicationZip);
            }
            finally
            {
                Response.Body.Dispose();
            }
        }





        #region 图片场景分析内部函数

        /// <summary>
        /// 获取分析结果中的图片说明
        /// </summary>
        /// <param name="analysis">图片分析结果</param>
        /// <returns></returns>
        private static List<string> GetCaptions(ImageAnalysis analysis)
        {
            var result = new List<string>();
            foreach (var caption in analysis.Description.Captions)
            {
                result.Add(caption.Text);
            }

            return result;
        }


        /// <summary>
        /// 获取分析结果中的图片标签和百分比
        /// </summary>
        /// <param name="analysis">图片分析结果</param>
        /// <returns></returns>
        private static List<ImgSceneRecognitionTagDto> GetImgTag(ImageAnalysis analysis)
        {
            var result = new List<ImgSceneRecognitionTagDto>();

            var tmpPercentage = 0.0;
            foreach (var tag in analysis.Tags)
            {
                tmpPercentage = Math.Round(tag.Confidence * 100, 2);
                result.Add(new ImgSceneRecognitionTagDto()
                {
                    TagName = tag.Name,
                    Percentage = tmpPercentage,
                    PercentageStr = $"{tmpPercentage}%"
                });
            }

            return result;
        }

        #endregion
    }
}