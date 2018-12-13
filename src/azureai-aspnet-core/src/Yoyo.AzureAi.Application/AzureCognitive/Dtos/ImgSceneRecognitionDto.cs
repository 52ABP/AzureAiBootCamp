using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.AzureAi.AzureCognitive.Dtos
{
    /// <summary>
    /// 图片场景识别Dto
    /// </summary>
    public class ImgSceneRecognitionDto
    {
        /// <summary>
        /// 图片场景说明
        /// </summary>
        public List<string> Captions { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public List<ImgSceneRecognitionTagDto> ImgTags { get; set; }


        /// <summary>
        /// 人脸
        /// </summary>
        public List<FaceDescription> Faces { get; set; }


        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }

    /// <summary>
    /// 图片场景识别图片标签和匹配度
    /// </summary>
    public class ImgSceneRecognitionTagDto
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 百分比(保留两位小数)
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        /// 百分比(例子:50.01%)
        /// </summary>
        public string PercentageStr { get; set; }
    }
}
