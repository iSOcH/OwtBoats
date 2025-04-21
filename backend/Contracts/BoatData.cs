using System.ComponentModel.DataAnnotations;

namespace backend.Contracts;

public class BoatData
{
    [MinLength(3), MaxLength(256)]
    public required string Name { get; set; }
    
    [MinLength(5), MaxLength(16384)]
    public string? Description { get; set; }
}