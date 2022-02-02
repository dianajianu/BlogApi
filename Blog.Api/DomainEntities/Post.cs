namespace Blog.Api
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }   
        
        public string? Image { get; set; }

        public virtual ICollection<Tag>? Tags { get; set; }
    }
}
