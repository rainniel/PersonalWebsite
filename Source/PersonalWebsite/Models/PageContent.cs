using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class PageContent
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public string? Content { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public PageContent() { }

        public PageContent(string content = "")
        {
            Content = content;
        }
    }
}
