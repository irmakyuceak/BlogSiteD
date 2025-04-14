namespace BlogSite.Models.View
{
    public class ArticleIndexViewModel
    {
        public List<Article> Articles { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
