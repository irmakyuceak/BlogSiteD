using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik boş olamaz.")]
        public string Content { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? ImagePath { get; set; } // Opsiyonel görsel

        public ICollection<Comment>? Comments { get; set; }
    }

}
