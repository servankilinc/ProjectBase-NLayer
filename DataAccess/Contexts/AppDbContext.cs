using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace DataAccess.Contexts;

public class AppDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid> // DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public override DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogLike> BlogLikes { get; set; }
    public DbSet<BlogComment> BlogComments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>(u =>
        {
            u.ToTable("dbo_user");
 
            u.HasMany(u => u.Blogs)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            u.HasMany(u => u.BlogComments)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            u.HasMany(u => u.BlogLikes)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            // ****** If SoftDeletable is used, the filter should be applied to all entities
            u.HasQueryFilter(f => !f.IsDeleted);
        });


        modelBuilder.Entity<Blog>(b =>
        {
            b.ToTable("dbo_blog");

            b.HasOne(b => b.Author)
               .WithMany(a => a.Blogs)
               .HasForeignKey(b => b.AuthorId)
               .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(b => b.Category)
               .WithMany(c => c.Blogs)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

            b.HasMany(b => b.BlogLikes)
                .WithOne(b => b.Blog)
                .HasForeignKey(b => b.BlogId)
                .OnDelete(DeleteBehavior.SetNull);
             
            b.HasMany(b => b.BlogComments)
               .WithOne(b => b.Blog)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.SetNull);
            
            b.HasQueryFilter(f => !f.IsDeleted);
        });


        modelBuilder.Entity<Category>(c =>
        {
            c.ToTable("dbo_category");

            c.HasMany(c => c.Blogs)
               .WithOne(b => b.Category)
               .HasForeignKey(b=> b.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

            c.HasQueryFilter(f => !f.IsDeleted);
        });

        modelBuilder.Entity<BlogLike>(b =>
        {
            b.ToTable("dbo_blogLike");

            b.HasOne(b => b.Blog)
               .WithMany(b => b.BlogLikes)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(b => b.User)
               .WithMany(u => u.BlogLikes)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.SetNull);
        });


        modelBuilder.Entity<BlogComment>(b =>
        {
            b.ToTable("dbo_blogComment");

            b.HasOne(b => b.User)
               .WithMany(u => u.BlogComments)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(b => b.Blog)
               .WithMany(b => b.BlogComments)
               .HasForeignKey(b => b.BlogId)
               .OnDelete(DeleteBehavior.SetNull);
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
