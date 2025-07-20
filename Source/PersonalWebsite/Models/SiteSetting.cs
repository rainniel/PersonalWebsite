using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class SiteSetting
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string SettingName { get; set; } = default!;

        public string Value { get; set; } = string.Empty;

        public DateTime? LastModifiedDateTime { get; set; }
    }
}
