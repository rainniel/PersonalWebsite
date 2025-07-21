using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class SocialMedia
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
        public string? URL { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public SocialMedia() { }

        public SocialMedia(string url = "", bool isHidden = false)
        {
            URL = url;
            IsHidden = isHidden;
        }

        public bool IsVisibleAndHasUrl()
        {
            return !string.IsNullOrWhiteSpace(URL) && !IsHidden;
        }
    }
}
