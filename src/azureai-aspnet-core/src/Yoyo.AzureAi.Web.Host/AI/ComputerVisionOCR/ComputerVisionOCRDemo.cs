using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoyo.AzureAi.Web.Host.AI.ComputerVisionOCR
{
    public class ComputerVisionOCRDemo
    {
        // Replace <Subscription Key> with your valid subscription key.
        const string apiKey = "";
        // api 地址
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";
        public static async Task<string> Run(string url)
        {
            HttpClient client = new HttpClient();
            var result = string.Empty;
            Console.WriteLine("从文件识别:");
            result = await AzureOcrHelper.OcrFromFile(client, uriBase, apiKey, url, "zh-Hans");
            Console.WriteLine("\r\n响应:\n\n{0}\n", JToken.Parse(result).ToString());
            result = AzureOcrHelper.SimpleFormattedText(result);
            Console.WriteLine("\r\n格式化输出结果:\n\n{0}\n", result);
            return result;
        }
    }
}
