using System.ComponentModel.DataAnnotations;

namespace backend.Contracts;

public class BoatInfo
{
    [MinLength(3)]
    public required string Name { get; set; }
    
    [MinLength(5)]
    public string? Description { get; set; }
}