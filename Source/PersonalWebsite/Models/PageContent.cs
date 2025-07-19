using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class PageContent
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string PageName { get; set; } = default!;

        public string Content { get; set; } = string.Empty;

        public DateTime? LastModified { get; set; }
    }
}
