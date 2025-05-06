using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts;

public class AppDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid> // DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogLikeMap> BlogLikeMaps { get; set; }
    public DbSet<BlogCommentMap> BlogCommentMaps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>(u =>
        {
            u.ToTable("Users");
 
            u.HasMany(u => u.Blogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            u.HasMany(u => u.BlogLikeMaps)
               .WithOne(blm => blm.User)
               .HasForeignKey(blm => blm.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            u.HasMany(u => u.BlogCommentMaps)
               .WithOne(bcm => bcm.User)
               .HasForeignKey(bcm => bcm.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            // ****** If SoftDeletable is used, the filter should be applied to all entities
            u.HasQueryFilter(f => !f.IsDeleted);
        });


        modelBuilder.Entity<Blog>(b =>
        {
            b.ToTable("Blogs");

            b.HasOne(b => b.User)
               .WithMany(u => u.Blogs)
               .HasForeignKey(b => b.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(b => b.BlogLikeMaps)
                .WithOne(blm => blm.Blog)
                .HasForeignKey(blm => blm.BlogId)
                .OnDelete(DeleteBehavior.Restrict);
             
            b.HasMany(b => b.BlogCommentMaps)
               .WithOne(bcm => bcm.Blog)
               .HasForeignKey(bcm => bcm.BlogId)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BlogLikeMap>(blm =>
        {
            blm.ToTable("BlogLikeMaps");

            blm.HasOne(blm => blm.User)
               .WithMany(u => u.BlogLikeMaps)
               .HasForeignKey(blm => blm.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            blm.HasOne(blm => blm.Blog)
               .WithMany(u => u.BlogLikeMaps)
               .HasForeignKey(blm => blm.BlogId)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BlogCommentMap>(bcm =>
        {
            bcm.ToTable("BlogCommentMap");

            bcm.HasOne(bcm => bcm.User)
               .WithMany(u => u.BlogCommentMaps)
               .HasForeignKey(bcm => bcm.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            bcm.HasOne(bcm => bcm.Blog)
               .WithMany(u => u.BlogCommentMaps)
               .HasForeignKey(bcm => bcm.BlogId)
               .OnDelete(DeleteBehavior.Restrict);
        });


        // If Identity Exist 
        // Remove IdentityRole defiantion if Custom Role Entity Exist
        modelBuilder.Entity<IdentityRole<Guid>>(entity => { entity.ToTable("Roles"); });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });
    }
}
