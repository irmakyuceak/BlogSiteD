using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);

            modelBuilder.Entity<Article>()
                 .HasOne(a => a.User)
                 .WithMany(u => u.Articles)
                 .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>().HasData(
                  new User
                  {
                      Id = 1,
                      Username = "testuser",
                      Email = "test@blog.com",
                      Password = "123",
                      Role = "User"
                  }
              );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Genel" },
                new Category { Id = 2, Name = "Teknoloji" },
                new Category { Id = 3, Name = "Günlük" }
            );

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    Title = "İlk Blog Yazım",
                    Content = "Bu bir örnek blog içeriğidir.",
                    UserId = 1,
                    CategoryId = 1,
                    PublishDate = new DateTime(2025, 4, 10)
                }
            );
        }

    }
}
