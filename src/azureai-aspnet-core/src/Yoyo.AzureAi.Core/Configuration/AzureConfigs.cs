using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.AzureAi.Configuration
{
    /// <summary>
    /// Azure相关的配置
    /// </summary>
    public class AzureConfigs
    {
        /// <summary>
        /// 计算机视觉OCR
        /// </summary>
        public AzureComputerVision ComputerVisionOCR { get; set; }

        /// <summary>
        /// 图片场景识别
        /// </summary>
        public AzureComputerVision ComputerVisionImgAnalyze { get; set; }

        /// <summary>
        /// 语音转文本
        /// </summary>
        public AzureSpeechToText SpeechToText { get; set; }

        /// <summary>
        /// 语音转文本
        /// </summary>
        public AzureTextToSpeech TextToSpeech { get; set; }
    }

    /// <summary>
    /// Azure在线服务配置实体
    /// </summary>
    public abstract class AuzreOnlineServiceBase
    {
        /// <summary>
        /// 订阅Key/ApiKey
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 端点Url
        /// </summary>
        public string Endpoint { get; set; }
    }

    /// <summary>
    /// 计算机视觉配置Model
    /// </summary>
    public class AzureComputerVision : AuzreOnlineServiceBase
    {

    }

    /// <summary>
    /// 语音转文本Model
    /// </summary>
    public class AzureSpeechToText : AuzreOnlineServiceBase
    {
        /// <summary>
        /// 转换接口
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 区域/地区
        /// </summary>
        public string Region { get; set; }
    }

    /// <summary>
    /// 文本转语音Model
    /// </summary>
    public class AzureTextToSpeech : AzureSpeechToText
    {
        /// <summary>
        /// Azure资源名称
        /// </summary>
        public string ResouceName { get; set; }

        /// <summary>
        /// 文本转语音模板
        /// </summary>
        public string ContentTemplate { get; set; }
    }
}
