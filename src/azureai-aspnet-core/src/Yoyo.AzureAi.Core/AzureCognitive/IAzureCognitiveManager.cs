using Abp.Runtime.Caching;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Yoyo.AzureAi.Configuration;
using Abp.Dependency;

namespace Yoyo.AzureAi.AzureCognitive
{
    public interface IAzureCognitiveManager : ITransientDependency
    {
        /// <summary>
        /// 图片场景分析
        /// </summary>
        /// <param name="imgUrl">图片地址</param>
        /// <param name="features">“特性”</param>
        /// <returns>图片分析结果</returns>
        Task<ImageAnalysis> ImgAnalyze(string imgUrl, List<VisualFeatureTypes> features = null);


        /// <summary>
        /// 图片OCR识别
        /// </summary>
        /// <param name="imgUrl">图片链接</param>
        /// <param name="language">语言</param>
        /// <param name="detectOrientation">是否启用方向检测(默认启用)</param>
        /// <returns>识别结果的json字符串</returns>
        Task<string> ImgOcr(string imgUrl, string language = "unk", bool detectOrientation = true);


        /// <summary>
        /// 图片OCR识别(简易转换)
        /// </summary>
        /// <param name="imgUrl">图片链接</param>
        /// <param name="language">语言</param>
        /// <param name="detectOrientation">是否启用方向检测(默认启用)</param>
        /// <returns>识别结果的json字符串</returns>
        Task<string> ImgOcrSimpleFormattedText(string imgUrl, string language = "unk", bool detectOrientation = true);


        /// <summary>
        /// Ocr识别结果json字符串简易格式化
        /// </summary>
        /// <param name="ocrJsonResult">识别结果json字符串</param>
        /// <returns>格式化之后的文字</returns>
        string SimpleFormattedText(string ocrJsonResult);


        /// <summary>
        /// 音频转文字
        /// </summary>
        /// <param name="audioStream">音频流</param>
        /// <param name="language">音频语言</param>
        /// <returns>识别结果</returns>
        Task<string> SpeechToText(Stream audioStream, string language);


        /// <summary>
        /// 文本转音频流(返回的流需要释放)
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="lang">语言</param>
        /// <param name="voice">说话人</param>
        /// <returns>音频流</returns>
        Task<MemoryStream> TextToSpeech(string text, string lang, string voice);
    }
}
