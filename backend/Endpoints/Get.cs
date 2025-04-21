using backend.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Endpoints;

public class Get
{
    public async Task<Results<Ok<BoatInfo>, NotFound>> GetBoat(Guid id)
    {
        // TODO retrieve
        // TODO return 404 if not found or found but not owner
        
        return TypedResults.Ok(new BoatInfo
        {
            Name = "Some boat"
        });
    }
}