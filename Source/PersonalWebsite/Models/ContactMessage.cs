using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string? Subject { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? IPAddress { get; set; }

        [MaxLength(200)]
        public string? UserAgent { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
