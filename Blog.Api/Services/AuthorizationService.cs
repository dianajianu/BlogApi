namespace Blog.Api.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public async Task<bool> IsAuthorized(IHeaderDictionary keyValuePairs, Utils.Enums.User user)
        {
            return keyValuePairs.TryGetValue("X-User", out var value) && value[0] == user.ToString();
        }
    }
}
