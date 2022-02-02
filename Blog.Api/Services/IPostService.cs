using Microsoft.AspNetCore.JsonPatch;

namespace Blog.Api.Services
{
    public interface IPostService
    {
        Task<Dto.Output.Post> AddPostAsync(Dto.Input.Post post);
        Task<Dto.Output.Post> UpdatePostAsync(Dto.Input.Post post);
        Task<Dto.Output.Post> PatchPostAsync(int id, JsonPatchDocument<Post> jsonPatch);
        Task DeletePostAsync(int postId);
        Task<Dto.Output.Post> GetPostAsync(int postId);
        Task<IEnumerable<Dto.Output.Post>> SearchPostAsync(string? title = null, int? categoryId = null, IEnumerable<int>? tags = null);
        Task<IEnumerable<Dto.Output.Category>> GetCategoriesAsync();
        Task<IEnumerable<Dto.Output.Tag>> GetTagsAsync();
    }
}
