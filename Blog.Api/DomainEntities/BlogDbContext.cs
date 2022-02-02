using Microsoft.EntityFrameworkCore;

namespace Blog.Api
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(){}

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed Author table
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Jane" });

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 2, Name = "Bob" });

            //Seed Category table
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Travel" });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 2, Name = "Food" });

            //Seed Post table
            modelBuilder.Entity<Post>().HasData(
                new Post 
                { 
                    Id = 1,
                    Title = "Europe",
                    CategoryId = 1,
                    AuthorId = 1,
                    Content = "From France's cultural attractions to Italy's wealth of historical sights and Germany's magnificent list of stunning architectural destinations, European countries have lots to offer visitors. As a result, picking the best attractions to visit can be extremely difficult",
                    Image = "https://i0.wp.com/files.tripstodiscover.com/files/2014/06/Cinque-Tierre.jpg"
                });

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 2,
                    Title = "Mediterranean food",
                    CategoryId = 2,
                    AuthorId = 2,
                    Content = "People around the Mediterranean have traditionally followed a diet that's rich in plant-based foods, including fruits, vegetables, whole grains, breads, legumes, potatoes, nuts, and seeds.",
                    Image = "https://www.al.com/resizer/PnbeNzRsNuHlykYEUkKp-HhYIJM=/1280x0/smart/arc-anglerfish-arc2-prod-advancelocal.s3.amazonaws.com/public/DKSBHVD54NCBHFRQBNM7Y4WFMY.jpg"
                });

            //Seed Tags table
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "Travel" },
                new Tag { Id = 2, Name = "Healthy life" },
                new Tag { Id = 3, Name = "Diet" });

            //Seed PostXTag table
            modelBuilder
                .Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(p => p.Posts)
                .UsingEntity(j => j.HasData(
                    new { PostsId = 1, TagsId = 1 },
                    new { PostsId = 1, TagsId = 2 },
                    new { PostsId = 2, TagsId = 2 },
                    new { PostsId = 2, TagsId = 3 }));
        }
    }
}
