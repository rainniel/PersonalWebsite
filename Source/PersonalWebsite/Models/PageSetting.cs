using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class PageSetting
    {

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public bool IsDisabled { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public PageSetting() { }

        public PageSetting(bool isDisabled)
        {
            IsDisabled = isDisabled;
        }
    }
}
