using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Authorization.Interfaces
{
    public interface IAccountManager
    {
        // User Manager
        Task<Tuple<bool, string[]>> CreateUserAsync(User user, IEnumerable<string> roles, string password);

        Task<User> GetUserByIdAsync(string userId);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<Tuple<User, string[]>> GetUserAndRolesAsync(int userId);

        Task<IList<string>> GetUserRolesAsync(User user);

        Task<List<Tuple<User, string[]>>> GetUsersAndRolesAsync(int page, int pageSize);

        // Role Manager
        Task<Tuple<bool, string[]>> CreateRoleAsync(Role role, IEnumerable<string> claims);

        Task<Role> GetRoleByIdAsync(string roleId);

        Task<Role> GetRoleByNameAsync(string roleName);

        Task<Role> GetRoleLoadRelatedAsync(string roleName);

        Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize);
    }
}
