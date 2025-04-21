namespace backend.Contracts;

public class BoatCreateRequest
{
    public required Guid Id { get; init; }
    
    public required BoatData Data { get; init; }
}