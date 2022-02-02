namespace Blog.Api.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsAuthorized(IHeaderDictionary keyValuePairs, Utils.Enums.User user);
    }
}
