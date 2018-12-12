using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yoyo.AzureAi.AzureCognitive.Dtos
{
    public class ImgOcrInput
    {
        /// <summary>
        /// 图片Url
        /// </summary>
        [Required]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Required]
        public string Lang { get; set; }
    }
}
