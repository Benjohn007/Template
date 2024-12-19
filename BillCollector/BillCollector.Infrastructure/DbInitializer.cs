using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core;
using BillCollector.Core.Entities;
using BillCollector.Core.Entities;
using BillCollector.Core.Enums;
using BillCollector.Infrastructure.DbContexts;

namespace BillCollector.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task InitializeDatabaseAsync(BillCollectorDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Add roles if they don't exist
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Name = "SuperAdmin",
                        Description = "Super Administrator with full system access",
                        CreatedBy = BillCollectorConstants.CREATED_BY_SYTEM
                    }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }


            // Assign permissions to roles if not already assigned
            if (!await context.Permissions.AnyAsync())
            {
                var superAdminRole = await context.Roles.FirstAsync(r => r.Name == "SuperAdmin");
                var permissions = new List<string> {"*" };

                // Assign all permissions to SuperAdmin
                var superAdminPermissions = permissions.Select(p => new Permission
                {
                    RoleId = superAdminRole.Id,
                    PermissionName = p,
                    CreatedBy = BillCollectorConstants.CREATED_BY_SYTEM
                });

                await context.Permissions.AddRangeAsync(superAdminPermissions);
                await context.SaveChangesAsync();
            }

            // Add default super admin user if no users exist
            if (!await context.Users.AnyAsync())
            {
                var superAdminRole = await context.Roles.FirstAsync(r => r.Name == "SuperAdmin");

                var superAdmin = new User
                {
                    Email = "admin@billcollector.com",
                    PhoneNumber = "2348166423487",
                    PhoneVerified = true,
                    PasswordHash = PasswordManager.Encrypt("Admin@123"),
                    FirstName = "System",
                    LastName = "Administrator",
                    Status = UserStatus.ACTIVE.ToString(),
                    EmailVerified = true,
                    CreatedBy = BillCollectorConstants.CREATED_BY_SYTEM
                };

                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                // Assign SuperAdmin role
                await context.UserRoles.AddAsync(new UserRole
                {
                    UserId = superAdmin.Id,
                    RoleId = superAdminRole.Id,
                    CreatedBy = BillCollectorConstants.CREATED_BY_SYTEM
                });

                await context.SaveChangesAsync();
            }
        }


        //THIS WAS INTENTIONALLY COMMENTED OUT BECAUSE OF THE DAMAGE IT CAN CAUSE
        //public static async Task ResetDatabaseAsync(BillCollectorDbContext context)
        //{
        //    // Be very careful with this method - it deletes all data!
        //    await context.Database.EnsureDeletedAsync();
        //    await InitializeDatabaseAsync(context);
        //}
    }
}
