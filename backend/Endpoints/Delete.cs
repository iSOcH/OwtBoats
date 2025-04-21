using backend.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Endpoints;

public class Delete
{
    public async Task<Results<Ok<BoatInfo>, NotFound>> DeleteBoat(Guid id)
    {
        // TODO check if boat exists
        // TODO check if we are owner
        return TypedResults.Ok(null as BoatInfo);
    }
}