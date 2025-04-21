using backend.Contracts;

namespace backend.Endpoints;

public class Index
{
    public Task<List<BoatDetail>> ListBoats()
    {
        List<BoatDetail> list = [new()
        {
            Id = Guid.NewGuid(),
            Boat = new() {
                Name = "MyBoat",
                Description = "Some description"
            }
        }];

        return Task.FromResult(list);
    }
}