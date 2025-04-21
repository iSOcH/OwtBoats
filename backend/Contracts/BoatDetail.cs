namespace backend.Contracts;

public class BoatDetail
{
    public required Guid Id { get; init; }
    
    public required BoatInfo Boat { get; init; }
}