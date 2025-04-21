using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class OwtBoatsDbContext(DbContextOptions<OwtBoatsDbContext> options)
    : IdentityDbContext<OwtBoatsUser>(options)
{
    public DbSet<Boat> Boats { get; set; }
}