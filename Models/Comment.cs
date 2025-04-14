using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Yorum yazan kişinin adı
        [Required]
        [StringLength(100)]
        public string AuthorName { get; set; }

        // Article ile ilişki
        public int ArticleId { get; set; }
        public Article? Article { get; set; }
    }
}
