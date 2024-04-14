

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
    public DbSet<UserCourseEntity> UserCourses { get; set; }
    public DbSet<ContactFormEntity> ContactForm { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserCourseEntity>()
            .HasKey(uc => new { uc.UserId, uc.CourseId });

        modelBuilder.Entity<UserCourseEntity>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCourses)
            .HasForeignKey(uc => uc.UserId);

        modelBuilder.Entity<UserCourseEntity>()
            .HasOne(uc => uc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(uc => uc.CourseId);
    }



}

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<UserEntity>()
    //        .HasMany(u => u.Adresses)
    //        .WithOne(a => a.User)
    //        .HasForeignKey(a => a.UserId)
    //        .OnDelete(DeleteBehavior.Restrict);
    //}

