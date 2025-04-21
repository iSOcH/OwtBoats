namespace backend.Contracts;

public class BoatCreateRequest
{
    public required Guid Id { get; init; }
    
    public required BoatInfo Boat { get; init; }
}