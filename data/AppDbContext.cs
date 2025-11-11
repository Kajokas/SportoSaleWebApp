using ispk.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ispk.data {

    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int> {
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Membership> Membership { get; set; } = null!;
        public DbSet<MembershipType> MembershipTypes { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder) {
	    base.OnModelCreating(builder);

	    builder.Entity<User>()
		.Property(u => u.role)
		.HasConversion<string>();

	    builder.Entity<User>()
                .HasOne(u => u.membership)
                .WithOne(m => m.user)
                .HasForeignKey<Membership>(m => m.userId)
                .OnDelete(DeleteBehavior.Cascade);

	    builder.Entity<MembershipType>()
		.HasMany(mt => mt.memberships)
		.WithOne(m => m.membershipType)
		.HasForeignKey(m => m.membershipTypeId)
		.OnDelete(DeleteBehavior.Restrict);
	}
    }
}
