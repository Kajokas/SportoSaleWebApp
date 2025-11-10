using ispk.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ispk.data {

    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int> {
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder) {
	    base.OnModelCreating(builder);

	    builder.Entity<User>()
		.Property(u => u.role)
		.HasConversion<string>();
	}
    }
}
