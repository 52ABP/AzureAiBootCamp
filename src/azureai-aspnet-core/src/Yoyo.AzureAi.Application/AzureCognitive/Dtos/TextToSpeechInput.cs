using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yoyo.AzureAi.AzureCognitive.Dtos
{
    public class TextToSpeechInput
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Required]
        public string Lang { get; set; }

        /// <summary>
        /// 发音人
        /// </summary>
        [Required]
        public string Voice { get; set; }
    }
}
