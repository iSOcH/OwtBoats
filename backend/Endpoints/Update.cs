using backend.Contracts;
using backend.Database;
using backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints;

public class Update(
    IValidator<BoatData> validator,
    OwtBoatsDbContext dbContext,
    IUserService userService)
{
    private readonly IValidator<BoatData> _validator = validator;
    private readonly OwtBoatsDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task<Results<Ok<BoatData>, NotFound, ValidationProblem>> UpdateBoat(Guid id, BoatData boatData)
    {
        var validationResult = await _validator.ValidateAsync(boatData);

        if (!validationResult.IsValid)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var userId = _userService.GetUserIdOrThrow();
        var dbBoat = await _dbContext.Boats.SingleOrDefaultAsync(b => b.Id == id && b.OwningUserId == userId);

        if (dbBoat is null)
            return TypedResults.NotFound();

        dbBoat.Name = boatData.Name;
        dbBoat.Description = boatData.Description;

        await _dbContext.SaveChangesAsync();
        
        return TypedResults.Ok(boatData);
    }
}