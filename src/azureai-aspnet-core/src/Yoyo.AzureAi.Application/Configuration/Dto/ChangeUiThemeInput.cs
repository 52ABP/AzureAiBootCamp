using System.ComponentModel.DataAnnotations;

namespace Yoyo.AzureAi.Configuration.Dto
{
    public class ChangeUiThemeInput
    {
        [Required]
        [StringLength(32)]
        public string Theme { get; set; }
    }
}
