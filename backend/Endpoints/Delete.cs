using backend.Contracts;
using backend.Database;
using backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints;

public class Delete(OwtBoatsDbContext dbContext, IUserService userService)
{
    private readonly OwtBoatsDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task<Results<Ok<BoatData>, NotFound>> DeleteBoat(Guid id)
    {
        var userId = _userService.GetUserIdOrThrow();
        var dbBoat = await _dbContext.Boats.SingleOrDefaultAsync(b => b.Id == id && b.OwningUserId == userId);

        if (dbBoat is null)
            return TypedResults.NotFound();

        _dbContext.Boats.Remove(dbBoat);
        await _dbContext.SaveChangesAsync();

        return TypedResults.Ok(new BoatData
        {
            Name = dbBoat.Name,
            Description = dbBoat.Description
        });
    }
}