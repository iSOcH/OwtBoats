using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

[Index(nameof(OwningUserId))]
public class Boat
{
    [Key]
    public Guid Id { get; init; }
    
    [MaxLength(256)]
    public required string Name { get; set; }
    
    [MaxLength(16384)]
    public string? Description { get; set; }
    
    // Deliberately not adding relation to OwtBoatsUser here in order to remain more flexible if user-management was changed (some
    // online resources even recommend having User-Management and Application entities in separate Dbs / DbContexts). 
    // The content of this field is fully application-controlled.
    [MaxLength(40)]
    public required string OwningUserId { get; init; }
}