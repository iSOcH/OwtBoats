namespace backend.Contracts;

public class BoatDetail
{
    public required Guid Id { get; init; }
    
    public required BoatData Data { get; init; }
}