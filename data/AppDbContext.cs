using Microsoft.EntityFrameworkCore;
using ispk.models;

namespace ispk.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
