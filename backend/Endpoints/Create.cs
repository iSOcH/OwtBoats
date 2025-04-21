using backend.Contracts;
using backend.Database;
using backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints;

public class Create(IValidator<BoatCreateRequest> boatDetailValidator, OwtBoatsDbContext dbContext, IUserService userService)
{
    private readonly IValidator<BoatCreateRequest> _boatDetailValidator = boatDetailValidator;
    private readonly OwtBoatsDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task<Results<Ok<BoatCreateRequest>, ValidationProblem, Conflict>> CreateBoat(BoatCreateRequest boat)
    {
        var validationResult = await _boatDetailValidator.ValidateAsync(boat);

        if (!validationResult.IsValid)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        if (await _dbContext.Boats.AnyAsync(b => b.Id == boat.Id))
        {
            // this technically leaks the existence of a boat with that id to users which do not own it
            return TypedResults.Conflict();
        }

        var userId = _userService.GetUserIdOrThrow();
        
        var dbBoat = new Boat
        {
            Id = boat.Id,
            Name = boat.Data.Name,
            Description = boat.Data.Description,
            OwningUserId = userId
        };

        _dbContext.Boats.Add(dbBoat);
        await _dbContext.SaveChangesAsync();

        return TypedResults.Ok(boat);
    }
}