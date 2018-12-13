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

namespace Yoyo.AzureAi.AzureCognitive
{
    public class AzureCognitiveManager : IAzureCognitiveManager
    {
        // 注意！在这里使用 HttpClient 发送请求,正式使用请优化


        // 图片分析指定要返回的特性
        private static readonly List<VisualFeatureTypes> _defaultFeatures =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        /* 音频格式化配置
         * 此配置不可更改，官方目前只支持此标准
         * 官方文档链接：https://docs.microsoft.com/zh-cn/azure/cognitive-services/speech-service/how-to-use-audio-input-streams
         * */
        private static AudioStreamFormat _audioStreamFormat = AudioStreamFormat.GetWaveFormatPCM(16000, 16, 1);



        private readonly AzureConfigs _azureConfigs;
        private readonly ICacheManager _cacheManager;

        public AzureCognitiveManager(AzureConfigs azureConfigs, ICacheManager cacheManager)
        {
            _azureConfigs = azureConfigs;
            _cacheManager = cacheManager;
        }


        #region 图片场景分析

        /// <summary>
        /// 图片场景分析
        /// </summary>
        /// <param name="imgUrl">图片地址</param>
        /// <param name="features">“特性”</param>
        /// <returns>图片分析结果</returns>
        public async Task<ImageAnalysis> ImgAnalyze(string imgUrl, List<VisualFeatureTypes> features = null)
        {
            // 创建分析器
            var computerVision = new ComputerVisionClient(
               new ApiKeyServiceClientCredentials(_azureConfigs.ComputerVisionImgAnalyze.ApiKey),
               new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = _azureConfigs.ComputerVisionImgAnalyze.Endpoint;

            if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            {
                throw new Abp.UI.UserFriendlyException($"无效的图片链接 {imgUrl}");
            }

            var analysisResult = await computerVision.AnalyzeImageAsync(imgUrl, features ?? _defaultFeatures);


            return analysisResult;
        }

        #endregion


        #region 图片OCR识别

        /// <summary>
        /// 图片OCR识别
        /// </summary>
        /// <param name="imgUrl">图片链接</param>
        /// <param name="language">语言</param>
        /// <param name="detectOrientation">是否启用方向检测(默认启用)</param>
        /// <returns>识别结果的json字符串</returns>
        public async Task<string> ImgOcr(string imgUrl, string language = "unk", bool detectOrientation = true)
        {
            if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            {
                throw new Abp.UI.UserFriendlyException($"无效的图片链接 {imgUrl}");
            }

            var url = $"{ _azureConfigs.ComputerVisionOCR.Endpoint}?language={language}&detectOrientation={detectOrientation.ToString()}";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _azureConfigs.ComputerVisionOCR.ApiKey);

                HttpResponseMessage response;
                var dataStr = JsonConvert.SerializeObject(new { url = imgUrl });

                using (StringContent content = new StringContent(dataStr))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(url, content);
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        /// <summary>
        /// 图片OCR识别(简易转换)
        /// </summary>
        /// <param name="imgUrl">图片链接</param>
        /// <param name="language">语言</param>
        /// <param name="detectOrientation">是否启用方向检测(默认启用)</param>
        /// <returns>识别结果的json字符串</returns>
        public async Task<string> ImgOcrSimpleFormattedText(string imgUrl, string language = "unk", bool detectOrientation = true)
        {
            var ocrResult = await ImgOcr(imgUrl, language, detectOrientation);
            var result = this.SimpleFormattedText(ocrResult);

            return result;
        }

        /// <summary>
        /// Ocr识别结果json字符串简易格式化
        /// </summary>
        /// <param name="ocrJsonResult">识别结果json字符串</param>
        /// <returns>格式化之后的文字</returns>
        public string SimpleFormattedText(string ocrJsonResult)
        {
            var objResult = JsonConvert.DeserializeObject<dynamic>(ocrJsonResult);

            if (objResult.regions == null)
            {
                return string.Empty;
            }


            var sb = new StringBuilder();
            foreach (var item in objResult.regions)
            {
                foreach (var item2 in item.lines)
                {
                    foreach (var item3 in item2.words)
                    {
                        sb.Append(item3.text);
                    }

                    sb.Append("\r\n");
                }
            }

            return sb.ToString();
        }

        #endregion



        #region 音频转文字

        /// <summary>
        /// 音频转文字
        /// </summary>
        /// <param name="audioStream">音频流</param>
        /// <param name="language">音频语言</param>
        /// <returns>识别结果</returns>
        public async Task<string> SpeechToText(Stream audioStream, string language)
        {
            var result = new StringBuilder();
            using (var audioConfig = AudioConfig.FromStreamInput(new ReadPCMStream(audioStream), _audioStreamFormat))
            {
                // 订阅信息配置
                var config = SpeechConfig.FromSubscription(this._azureConfigs.SpeechToText.ApiKey, this._azureConfigs.SpeechToText.Region);
                // 语言配置
                config.SpeechRecognitionLanguage = language;
                // 此处作为停止器
                var stopRecognition = new TaskCompletionSource<int>();
                // 创建分析器
                using (var recognizer = new SpeechRecognizer(config, audioConfig))
                {
                    // 订阅分析事件
                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            result.AppendLine(e.Result.Text);
                        }

                    };
                    recognizer.Canceled += (s, e) =>
                    {
                        if (e.Reason == CancellationReason.Error)
                        {
                            result.AppendLine($"识别取消: 错误码={e.ErrorCode}");
                            result.AppendLine($"识别取消: 错误详情={e.ErrorDetails}");
                            result.AppendLine($"识别取消: 请检查你的Azure订阅是否更新");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    // 开始连续的识别。使用stopcontinuousrecognition()来停止识别。
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // 等待完成。
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // 停止识别
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }

            return result.ToString();
        }

        #endregion

        #region 文本转音频

        /// <summary>
        /// 文本转音频流(返回的流需要释放)
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="lang">语言</param>
        /// <param name="voice">说话人</param>
        /// <returns>音频流</returns>
        public async Task<byte[]> TextToSpeech(string text, string lang, string voice)
        {
            var accessToken = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this._azureConfigs.TextToSpeech.ApiKey);
                    UriBuilder uriBuilder = new UriBuilder(this._azureConfigs.TextToSpeech.Endpoint);

                    var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                    accessToken = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                throw new Abp.UI.UserFriendlyException($"获取Token失败,错误信息:{e.Message}");
            }

            // 创建请求内容
            var body = string.Format(this._azureConfigs.TextToSpeech.ContentTemplate, lang, voice, text);


            // 发送请求
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    // 设置请求方式为post
                    request.Method = HttpMethod.Post;
                    // 设置url
                    request.RequestUri = new Uri(this._azureConfigs.TextToSpeech.Host);
                    // 设置content类型
                    request.Content = new StringContent(body, Encoding.UTF8, "application/ssml+xml");
                    // 设置token
                    request.Headers.Add("Authorization", "Bearer " + accessToken);
                    request.Headers.Add("Connection", "Keep-Alive");
                    // 设置资源名称
                    request.Headers.Add("User-Agent", this._azureConfigs.TextToSpeech.ResouceName);
                    // 输出音频
                    request.Headers.Add("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
                    // 创建请求
                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        // 异步读取响应
                        using (var dataStream = await response.Content.ReadAsStreamAsync())
                        {
                            byte[] buffer = new byte[dataStream.Length];

                            //StreamContent a = new StreamContent(dataStream);
                            dataStream.Read(buffer, 0, buffer.Length);


                            return buffer;
                            //var stream = new MemoryStream();
                            //await dataStream.CopyToAsync(stream);
                            //return dataStream;
                        }
                    }
                }
            }
        }


        #endregion

    }
}
