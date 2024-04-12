

using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<AdressEntity> Adresses { get; set; }
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<SubscriberEntity> Subscribers { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<UserEntity>()
    //        .HasMany(u => u.Adresses)
    //        .WithOne(a => a.User)
    //        .HasForeignKey(a => a.UserId)
    //        .OnDelete(DeleteBehavior.Restrict);
    //}
}
