using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkWebApp.Models.DBModels;
using System;

namespace SocialNetworkWebApp.Data
{
    public class SocialNetworkDBContext : IdentityDbContext<User, Role, Guid>
    {
        public SocialNetworkDBContext(DbContextOptions<SocialNetworkDBContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var adminRoleGuid = Guid.NewGuid();
            var userRoleGuid = Guid.NewGuid();
            var adminUserGuid = Guid.NewGuid();
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleGuid,
                    Name = "Admin",
                    Description = "Admin role",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = userRoleGuid,
                    Name = "User",
                    Description = "User role",
                    NormalizedName = "USER"
                }
            );
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserGuid,
                    UserName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "Admin123!@#"),
                    FullName = "Administrator",
                    NormalizedUserName = "admin",
                    Email = "admin@email.com",
                    NormalizedEmail = "admin@email.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = adminUserGuid,
                    RoleId = adminRoleGuid,
                }
            );


            builder.Entity<Friendship>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder.Entity<Friendship>()
                .HasKey(bc => new { bc.FromUserId, bc.ToUserId });
            builder.Entity<Friendship>()
                .HasOne(bc => bc.FromUser)
                .WithMany(b => b.SentRequests)
                .HasForeignKey(bc => bc.FromUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Friendship>()
                .HasOne(bc => bc.ToUser)
                .WithMany(c => c.ReceivedRequests)
                .HasForeignKey(bc => bc.ToUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<Friendship> Friendships { get; set; }
    }
}
