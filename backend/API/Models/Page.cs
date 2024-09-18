using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("pages")]
    public class Page
    {
        [Key]
        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Url]
        [Column("url")]
        public string Url { get; set; }

        [Required]
        [StringLength(10)]
        [Column("language")]
        public string Language { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }
        
        [Required]
        [Column("content")]
        public string Content { get; set; }
    }
}
