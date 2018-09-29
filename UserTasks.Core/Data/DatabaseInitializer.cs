using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserTasks.Core.Authorization;
using UserTasks.Core.Authorization.Interfaces;
using UserTasks.Core.Data.Interfaces;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(AppDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating users account");

                const string adminRoleName = RoleName.Admin;

                const string supportRoleName = RoleName.Support;

                await EnsureRoleAsync(adminRoleName, ApplicationPermissions.GetAllPermissionValues());

                await EnsureRoleAsync(supportRoleName, new string[] { });

                await CreateUserAsync("admin", "Admin@123", "Administrator", "admin@nasr.com", new[] { adminRoleName });

                var supportUser = await CreateUserAsync("support", "Support@123", "Support User", "support@nasr.com", new[] { supportRoleName });

                var supportUser2 = await CreateUserAsync("support2", "Support@123", "Support User 2", "support2@nasr.com", new[] { supportRoleName });

                _logger.LogInformation("User accounts generation completed");

                if (await _context.Users.AnyAsync() && !await _context.TaskItems.AnyAsync())
                {
                    _logger.LogInformation("Seeding initial data");

                    var tasks = new List<TaskItem>
                    {
                        new TaskItem
                        {
                            Task = "Task item one",
                            UserOwnerId = supportUser.Id,
                            //  DateCreated = DateTime.UtcNow
                        },
                        new TaskItem
                        {
                            Task = "Task item two",
                            UserOwnerId = supportUser.Id
                        },
                        new TaskItem
                        {
                            Task = "Task item three",
                            UserOwnerId = supportUser2.Id
                        },
                        new TaskItem
                        {
                            Task = "Task item four",
                            UserOwnerId = supportUser2.Id
                        },
                        new TaskItem
                        {
                            Task = "Task item five",
                            UserOwnerId = supportUser.Id
                        },
                    };

                    _context.TaskItems.AddRange(tasks);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Seeding initial data completed");
                }
            }
        }

        private async Task EnsureRoleAsync(string roleName, IEnumerable<string> claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                var role = new Role(roleName);

                var result = await _accountManager.CreateRoleAsync(role, claims);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{roleName}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<User> CreateUserAsync(string userName, string password, string fullName, string email, IEnumerable<string> roles)
        {
            var user = new User
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await _accountManager.CreateUserAsync(user, roles, password);

            if (!result.Item1)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");

            return user;
        }
    }

}
