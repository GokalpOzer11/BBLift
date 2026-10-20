using BBLift.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Infrastructure.Persistence.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions options) : base(options){}
    protected AppDbContext() {}
    
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<FavoriteExercise> FavoriteExercises { get; set; }
    public DbSet<MuscleGroup> MuscleGroups { get; set; }
    public DbSet<Routine> Routines { get; set; }
    public DbSet<RoutineExercise> RoutineExercises { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<WorkoutLog> WorkoutLogs { get; set; }
    public DbSet<MembershipPackage> MembershipPackages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<TokenInfo> TokenInfos => Set<TokenInfo>();
    public DbSet<Payment> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<AppUser>().ToTable("Users", "auth");
        builder.Entity<AppRole>().ToTable("Roles", "auth");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "auth");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "auth");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "auth");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "auth");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "auth");

        
        builder.Entity<AppUser>(b =>
        {
            b.OwnsOne(x => x.Height, h =>
            {
                h.Property(p => p.Value).HasColumnName("HeightValue").IsRequired();
                h.Property(p => p.Unit).HasColumnName("HeightUnit").IsRequired();
            });
            b.Navigation(x => x.Height).IsRequired();

            b.OwnsOne(x => x.Weight, w =>
            {
                w.Property(p => p.Value).HasColumnName("WeightValue").IsRequired();
                w.Property(p => p.Unit).HasColumnName("WeightUnit").IsRequired();
            });
            b.Navigation(x => x.Weight).IsRequired();
        });
        builder.Entity<RoutineExercise>(b =>
        {
            b.OwnsOne(x => x.RestTime);
            b.OwnsOne(x => x.Weight); 
        });
        builder.Entity<WorkoutSession>(b =>
        {
            b.OwnsOne(x => x.WorkOutDuration);
        });
    }
}