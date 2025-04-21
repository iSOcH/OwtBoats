using backend.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Endpoints;

public class Create(IValidator<BoatCreateRequest> boatDetailValidator)
{
    private readonly IValidator<BoatCreateRequest> _boatDetailValidator = boatDetailValidator;

    public async Task<Results<Ok<BoatCreateRequest>, ValidationProblem, Conflict>> CreateBoat(BoatCreateRequest boat)
    {
        var validationResult = await _boatDetailValidator.ValidateAsync(boat);

        if (!validationResult.IsValid)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        
        var result = TypedResults.Ok(boat);
        return result;
    }
}