using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Dto.Input
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MaxLength(1024, ErrorMessage = "Content must not be more than 1024 characters")]
        public string Content { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "AuthorId is required")]
        public int AuthorId { get; set; }

        public string? Image { get; set; }

        public List<int>? Tags { get; set; }
    }
}