using backend.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Endpoints;

public class Update(IValidator<BoatInfo> validator)
{
    private readonly IValidator<BoatInfo> _validator = validator;

    public async Task<Results<Ok<BoatInfo>, NotFound, ValidationProblem>> UpdateBoat(Guid id, BoatInfo boatInfo)
    {
        var validationResult = await _validator.ValidateAsync(boatInfo);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        // TODO actually load and update
        
        return TypedResults.Ok(boatInfo);
    }
}