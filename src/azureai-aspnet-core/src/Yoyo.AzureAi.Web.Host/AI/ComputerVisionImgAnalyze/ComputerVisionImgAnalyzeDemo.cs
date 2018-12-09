using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yoyo.AzureAi.Web.Host.AI.ComputerVisionImgAnalyze
{
    public class ComputerVisionImgAnalyzeDemo
    {
        private const string apiKey = "";

        // 指定要返回的特性
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        public static async Task<string> Run(string url)
        {
            var computerVision = new ComputerVisionClient(
                            new ApiKeyServiceClientCredentials(apiKey),
                            new System.Net.Http.DelegatingHandler[] { });

            // 指定Azure区域
            computerVision.Endpoint = "https://westcentralus.api.cognitive.microsoft.com";

            // 远程图片
            return await AnalyzeRemoteAsync(computerVision, url);
        }
        
        // 分析远程图像
        private static async Task<string> AnalyzeRemoteAsync(
            ComputerVisionClient computerVision, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                return string.Format("\n无效的图片链接:\n{0} \n", imageUrl);
            }

            ImageAnalysis analysis = await computerVision.AnalyzeImageAsync(imageUrl, features);

            string r1= DisplayResults(analysis, imageUrl);
            string r2= DisplayImgTag(analysis);
            return r1 + r2;
        }

        // 分析本地图像
        private static async Task AnalyzeLocalAsync(
            ComputerVisionClient computerVision, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("\n无法打开或读取本地图像路径:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);

                DisplayResults(analysis, imagePath);
                DisplayImgTag(analysis);
            }
        }

        // 显示图像最相关的标题
        private static string DisplayResults(ImageAnalysis analysis, string imageUri)
        {
            string r = string.Format("\r\n\r\n{0}", imageUri);
            foreach (var caption in analysis.Description.Captions)
            {
                r+=string.Format("\r\n{0}\r\n", caption.Text);
            }
            return r;
        }

        // 展示标签
        private static string DisplayImgTag(ImageAnalysis analysis)
        {
            string r = "";
            foreach (var tag in analysis.Tags)
            {
                r += string.Format("标签名称:{0}       {1}%", tag.Name, Math.Round(tag.Confidence * 100, 2));
            }
            return r;
        }
    }
}
