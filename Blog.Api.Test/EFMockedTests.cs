using Blog.Api.Services;
using Blog.Api.Test.AsyncQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Test
{
    [TestClass]
    public class EFMockedTests
    {
        [TestMethod]
        public async Task Create_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(m => m.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var newlyAddedPost = await service.AddPostAsync(new Dto.Input.Post
            {
                Id = 1,
                AuthorId = 1,
                CategoryId = 1,
                Title = "Testing framework",
                Content = "A testing framework is a set of guidelines or rules used for creating and designing test cases.",
                Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/Selenium_Logo.png/220px-Selenium_Logo.png"
            });

            mockSet.Verify(m => m.AddAsync(It.IsAny<Post>(), default), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [TestMethod]
        public async Task Update_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var updatedPost = await service.UpdatePostAsync(new Dto.Input.Post
            {
                Id = 1,
                AuthorId = 2,
                CategoryId = 2,
                Title = "Drinks",
                Content = "Wine is an alcoholic drink typically made from fermented grapes.",
                Image = "https://253qv1sx4ey389p9wtpp9sj0-wpengine.netdna-ssl.com/wp-content/uploads/2021/10/HERO_Strcutural_Elements_of-Wine_GettyImages-1233242907_1920x1280-700x461.jpg"
            });

            Assert.AreEqual(updatedPost.Id, 1);
            Assert.AreEqual(updatedPost.Title, "Drinks");
            Assert.AreEqual(updatedPost.Content, "Wine is an alcoholic drink typically made from fermented grapes.");
            Assert.AreEqual(updatedPost.Image, "https://253qv1sx4ey389p9wtpp9sj0-wpengine.netdna-ssl.com/wp-content/uploads/2021/10/HERO_Strcutural_Elements_of-Wine_GettyImages-1233242907_1920x1280-700x461.jpg");
        }

        [TestMethod]
        public async Task Assign_Category_To_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(m => m.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);

            var js = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Post>();
            js.Operations.Add(new Microsoft.AspNetCore.JsonPatch.Operations.Operation<Post>
            {
                op = "replace",
                path = "CategoryId",
                value = 2
            });
            var patchedPost = await service.PatchPostAsync(1, js);

            Assert.AreEqual(patchedPost.Category.Id, 2);
        }

        [TestMethod]
        public async Task Add_Tag_To_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(m => m.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);

            var js = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Post>();
            js.Operations.Add(new Microsoft.AspNetCore.JsonPatch.Operations.Operation<Post>
            {
                op = "replace",
                path = "Tags",
                value = Tags.Where(tag => tag.Name == "Healthy life" || Posts.First(post => post.Id == 1).Tags.Any(t => t.Id == tag.Id))
            });
            var patchedPost = await service.PatchPostAsync(1, js);

            Assert.IsTrue(patchedPost.Tags.Any(tag => tag.Name == "Healthy life"));
        }

        [TestMethod]
        public async Task Remove_Tag_From_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(m => m.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);

            var js = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Post>();
            js.Operations.Add(new Microsoft.AspNetCore.JsonPatch.Operations.Operation<Post>
            {
                op = "replace",
                path = "Tags",
                value = Posts.First(post => post.Id == 2).Tags.Where(t => t.Name != "Healthy life")
            });
            var patchedPost = await service.PatchPostAsync(2, js);

            Assert.IsFalse(patchedPost.Tags.Any(tag => tag.Name == "Healthy life"));
        }

        [TestMethod]
        public async Task Get_All_Posts()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var posts = (await service.SearchPostAsync()).ToList();

            Assert.AreEqual(2, posts.Count);
            Assert.AreEqual("Travel", posts[0].Title);
            Assert.AreEqual("Food", posts[1].Title);
        }

        [TestMethod]
        public async Task Get_Posts_By_Title()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var posts = (await service.SearchPostAsync(title: "Travel")).ToList();

            foreach (var post in posts)
            {
                Assert.AreEqual("Travel", post.Title);
            }
        }

        [TestMethod]
        public async Task Get_Posts_By_Title_And_Category_And_Tags()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Post>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Post>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var posts = (await service.SearchPostAsync(title: "Food", categoryId: 2, tags: Tags.Where(tag => tag.Name == "Healthy life").Select(tag => tag.Id))).ToList();

            foreach (var post in posts)
            {
                Assert.AreEqual("Food", post.Title);
                Assert.AreEqual("Food", post.Category.Name);
                Assert.IsTrue(post.Tags.Any(tag => tag.Name == "Healthy life"));
            }
        }

        [TestMethod]
        public async Task Get_All_Categories()
        {
            var mockSet = new Mock<DbSet<Category>>();

            mockSet.As<IAsyncEnumerable<Category>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Category>(Categories.GetEnumerator()));

            mockSet.As<IQueryable<Category>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Category>(Categories.Provider));

            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(Categories.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(Categories.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(Categories.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var posts = (await service.GetCategoriesAsync()).ToList();

            Assert.AreEqual(2, posts.Count);
        }

        [TestMethod]
        public async Task Get_All_Tags()
        {
            var mockSet = new Mock<DbSet<Tag>>();

            mockSet.As<IAsyncEnumerable<Tag>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Tag>(Tags.GetEnumerator()));

            mockSet.As<IQueryable<Category>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Category>(Tags.Provider));

            mockSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(Tags.Expression);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(Tags.ElementType);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(Tags.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Tags).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            var posts = (await service.GetTagsAsync()).ToList();

            Assert.AreEqual(3, posts.Count);
            Assert.AreEqual("Travel", posts[0].Name);
            Assert.AreEqual("Food", posts[1].Name);
            Assert.AreEqual("Healthy life", posts[2].Name);
        }

        [TestMethod]
        public async Task Delete_Post()
        {
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.As<IAsyncEnumerable<Post>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestDbAsyncEnumerator<Post>(Posts.GetEnumerator()));

            mockSet.As<IQueryable<Category>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Category>(Posts.Provider));

            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(Posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(Posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(Posts.GetEnumerator());

            var mockContext = new Mock<BlogDbContext>();
            mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            var service = new PostService(mockContext.Object);
            await service.DeletePostAsync(Posts.First().Id);

            mockContext.Verify(m => m.Remove(It.IsAny<Post>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        private IQueryable<Post> Posts => new List<Post>
            {
                new Post
                {
                    Id = 1,
                    AuthorId = 1,
                    Author = Authors.First(author => author.Id == 1),
                    CategoryId = 1,
                    Category = Categories.First(category => category.Id == 1),
                    Title = "Travel",
                    Content = "Some of the most beautiful things to do",
                    Image = "https://i0.wp.com/files.tripstodiscover.com/files/2014/06/Cinque-Tierre.jpg",
                    Tags = Tags.Where(tag => tag.Name.Contains("Travel")).ToList() 
                },
                new Post
                {
                    Id = 2,
                    AuthorId = 2,
                    Author = Authors.First(author => author.Id == 2),
                    CategoryId = 2,
                    Category = Categories.First(category => category.Id == 2),
                    Title = "Food",
                    Content = "Chose wisely it sustain your health",
                    Image = "https://www.al.com/resizer/PnbeNzRsNuHlykYEUkKp-HhYIJM=/1280x0/smart/arc-anglerfish-arc2-prod-advancelocal.s3.amazonaws.com/public/DKSBHVD54NCBHFRQBNM7Y4WFMY.jpg",
                    Tags = Tags.Where(tag => tag.Name.Contains("Healthy life") || tag.Name.Contains("Food")).ToList()
                },
            }.AsQueryable();

        private IQueryable<Category> Categories => new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Travel"
                },
                new Category
                {
                    Id = 2,
                    Name = "Food"
                },
            }.AsQueryable();

        private IQueryable<Tag> Tags => new List<Tag>
            {
                new Tag
                {
                    Id = 1,
                    Name = "Travel"
                },
                new Tag
                {
                    Id = 2,
                    Name = "Food"
                },
                new Tag
                {
                    Id = 3,
                    Name = "Healthy life"
                }
            }.AsQueryable();

        private IQueryable<Author> Authors => new List<Author>
        {
              new Author
              {
                  Id = 1,
                  Name = "Jane"
              },
              new Author
              {
                  Id= 2,
                  Name = "Bob"
              }
        }.AsQueryable();
    }
}