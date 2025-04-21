namespace backend.Services;

public interface IUserService
{
    string? TryGetUserId();
    
    string GetUserIdOrThrow();
}