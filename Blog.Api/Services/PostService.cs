using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _blogDbContext;

        public PostService(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Dto.Output.Post> AddPostAsync(Dto.Input.Post post)
        {
            var result = await _blogDbContext.Posts.AddAsync(new Post 
            {
                CategoryId = post.CategoryId,
                AuthorId = post.AuthorId,   
                Title = post.Title,
                Content = post.Content,
                Image = post.Image,
                Tags = post.Tags != null && post.Tags.Any()
                     ?_blogDbContext.Tags.Where(tag => post.Tags.Any(pt => pt == tag.Id)).ToList()
                     : null,
            });

            await _blogDbContext.SaveChangesAsync();

            if (result?.Entity != null)
            {
                var references = _blogDbContext.Entry(result.Entity).References;
                foreach (var reference in references)
                    reference.Load();

                return ToDtoOutputPost(result.Entity);
            }

            return null;
        }

        public async Task<Dto.Output.Post> UpdatePostAsync(Dto.Input.Post post)
        {
            var result = await _blogDbContext.Posts
                .Where(p => p.Id == post.Id)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync();

            if (result != null)
            {
                result.CategoryId = post.CategoryId;
                result.AuthorId = post.AuthorId;
                result.Title = post.Title;
                result.Content = post.Content;
                result.Image = post.Image;
                result.Tags = post.Tags != null && post.Tags.Any()
                            ? _blogDbContext.Tags.Where(tag => post.Tags.Contains(tag.Id)).ToList()
                            : null;

                await _blogDbContext.SaveChangesAsync();

                var references = _blogDbContext.Entry(result)?.References;
                if (references != null)
                {
                    foreach (var reference in references)
                        reference.Load();
                }

                return ToDtoOutputPost(result);
            }

            return null;
        }

        public async Task<Dto.Output.Post> PatchPostAsync(int id, JsonPatchDocument<Post> jsonPatch)
        {
            var post = await _blogDbContext.Posts
                    .Where(post => post.Id == id)
                    .FirstOrDefaultAsync();

            if (post != null)
            {
                jsonPatch.ApplyTo(post);

                await _blogDbContext.SaveChangesAsync();

                var references = _blogDbContext.Entry(post)?.References;
                if (references != null)
                {
                    foreach (var reference in references)
                        reference.Load();
                }
                return ToDtoOutputPost(post);
            }

            return null;
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _blogDbContext.Posts
                    .Where(post => post.Id == postId)
                    .FirstOrDefaultAsync();

            if (post != null)
            {
                _blogDbContext.Remove(post);

                await _blogDbContext.SaveChangesAsync();
            }
        }

        public async Task<Dto.Output.Post> GetPostAsync(int postId)
        {
            var post = await _blogDbContext.Posts
                    .Where(post => post.Id == postId)
                    .FirstOrDefaultAsync();

            if (post != null)
            {
                var references = _blogDbContext.Entry(post).References;
                foreach (var reference in references)
                    reference.Load();

                return ToDtoOutputPost(post);
            }

            return null;
        }

        public async Task<IEnumerable<Dto.Output.Post>> SearchPostAsync(string? title = null, int? categoryId = null, IEnumerable<int>? tags = null)
        {
            IQueryable<Post> posts = _blogDbContext.Posts
                         .Include(post => post.Category)
                         .Include(post => post.Author)
                         .Include(post => post.Tags);

            if (categoryId != null && categoryId > 0)
            {
                posts = posts.Where(post => post.CategoryId == categoryId);
            }
            if (!string.IsNullOrEmpty(title))
            {
                posts = posts.Where(post => post.Title.Contains(title));
            }
            if(tags != null && tags.Any())
            {
                posts = posts.Where(post => post.Tags.Any(tag => tags.Contains(tag.Id)));
            }

            return (await posts.ToListAsync()).Select(post => ToDtoOutputPost(post));
        }

        public async Task<IEnumerable<Dto.Output.Category>> GetCategoriesAsync()
        {
            return (await _blogDbContext.Categories.ToListAsync())
                   .Select(category => new Dto.Output.Category
                   {
                        Id = category.Id,
                        Name = category.Name
                   });
        }

        public async Task<IEnumerable<Dto.Output.Tag>> GetTagsAsync()
        {
            return (await _blogDbContext.Tags.ToListAsync())
                   .Select(tag => new Dto.Output.Tag
                   {
                       Id = tag.Id,
                       Name = tag.Name
                   });
        }

        private Dto.Output.Post ToDtoOutputPost(Post post)
        {
            return new Dto.Output.Post
            {
                Id = post.Id,
                Author = post.Author != null
                ? new Dto.Output.Author
                  {
                      Id = post.AuthorId,
                      Name = post.Author.Name
                  }
                : null,
                Category = post.Category != null
                ? new Dto.Output.Category
                  {
                      Id = post.CategoryId,
                      Name = post.Category.Name
                  }
                : null,
                Title = post.Title,
                Content = post.Content,
                Image = post.Image,
                Tags = post != null
                ? post.Tags?.Select(tag => new Dto.Output.Tag
                  {
                      Id = tag.Id,
                      Name = tag.Name,
                  }).ToList()
                : null,
            };
        }
    }
}
