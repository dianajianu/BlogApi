namespace Blog.Api.Dto.Output
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }
        public string? Image { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
