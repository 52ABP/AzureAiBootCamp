using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yoyo.AzureAi.AzureCognitive.Dtos
{
    public class ImgSceneRecognitionInput
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [Required]
        public string ImgUrl { get; set; }
    }
}
