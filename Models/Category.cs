using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; }

        public ICollection<Article>? Articles { get; set; }
    }

}
