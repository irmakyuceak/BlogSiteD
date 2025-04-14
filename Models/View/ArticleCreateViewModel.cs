using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.View
{
    public class ArticleCreateViewModel
    {
        public Article? Article { get; set; }
        public List<Category>? Categories { get; set; }

        [Required(ErrorMessage = "Görsel seçimi zorunludur.")]
        public IFormFile ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }
    }
}
