using System.Security.Claims;

namespace backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? TryGetUserId()
    {
        if (_httpContextAccessor.HttpContext is not { } httpContext)
            throw new InvalidOperationException("This service can only be used during Http request");
        
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }

    public string GetUserIdOrThrow()
    {
        var id = TryGetUserId();
        
        if (id is null)
            throw new InvalidOperationException("User is not logged in");

        return id;
    }
}