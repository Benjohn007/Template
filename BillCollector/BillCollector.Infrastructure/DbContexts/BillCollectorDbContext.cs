using Microsoft.EntityFrameworkCore;
//using BillCollector.Core.Entities;
using BillCollector.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core.Entities;

namespace BillCollector.Infrastructure.DbContexts
{
    public class BillCollectorDbContext : DbContext
    {
        public BillCollectorDbContext(DbContextOptions<BillCollectorDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure composite keys
            //modelBuilder.Entity<UserRole>()
            //    .HasKey(ur => new { ur.UserId, ur.RoleId });


            //// Configure relationships
            //modelBuilder.Entity<UserRole>()
            //    .HasOne(ur => ur.User)
            //    .WithMany(u => u.UserRoles)
            //    .HasForeignKey(ur => ur.UserId);

            //modelBuilder.Entity<UserRole>()
            //    .HasOne(ur => ur.Role)
            //    .WithMany(r => r.UserRoles)
            //    .HasForeignKey(ur => ur.RoleId);

            //modelBuilder.Entity<Permission>()
            //    .HasOne(rp => rp.Role)
            //    .WithMany(r => r.RolePermissions)
            //    .HasForeignKey(rp => rp.RoleId);

            //modelBuilder.Entity<Permission>()
            //    .HasOne(rp => rp.PermissionName)
            //    .WithMany(p => p.RolePermissions)
            //    .HasForeignKey(rp => rp.PermissionId);

            // Configure indexes
            //modelBuilder.Entity<User>()
            //    .HasIndex(u => u.Email)
            //    .IsUnique();

            //modelBuilder.Entity<UserSession>()
            //    .HasIndex(us => us.Token)
            //    .IsUnique();

            //modelBuilder.Entity<UserSession>()
            //    .HasIndex(us => us.UserId);

            //modelBuilder.Entity<UserAuditLog>()
            //    .HasIndex(al => al.UserId);

            //modelBuilder.Entity<UserAuditLog>()
            //    .HasIndex(al => al.CreatedAt);

            //// Configure default values for MySQL
            //modelBuilder.Entity<User>()
            //    .Property(u => u.CreatedAt)
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            //modelBuilder.Entity<User>()
            //    .Property(u => u.UpdatedAt)
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

            //modelBuilder.Entity<User>()
            //    .Property(u => u.Status)
            //    .HasConversion<string>()
            //    .HasMaxLength(20);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;

                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;

                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        #region DB Sets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

    }
}
