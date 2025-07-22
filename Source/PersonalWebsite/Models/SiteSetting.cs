using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Models
{
    public class SiteSetting
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public string? Value { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public SiteSetting() { }

        public SiteSetting(string value)
        {
            Value = value;
        }

        public SiteSetting(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
