using System.ComponentModel.DataAnnotations;

namespace Yoyo.AzureAi.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}