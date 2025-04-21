using backend.Contracts;
using backend.Database;
using backend.Services;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints;

public class Index(OwtBoatsDbContext dbContext, IUserService userService)
{
    private readonly OwtBoatsDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task<IReadOnlyList<BoatDetail>> ListBoats()
    {
        var userId = _userService.GetUserIdOrThrow();
        var dbBoats = _dbContext.Boats.Where(b => b.OwningUserId == userId);

        var mappedBoats = await dbBoats
            .Select(b => new BoatDetail
            {
                Id = b.Id,
                Data = new BoatData
                {
                    Name = b.Name,
                    Description = b.Description
                }
            })
            .ToArrayAsync();

        return mappedBoats;
    }
}