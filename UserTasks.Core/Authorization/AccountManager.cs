using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserTasks.Core.Authorization.Interfaces;
using UserTasks.Core.Data;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Authorization
{
    public class AccountManager : IAccountManager
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public AccountManager(AppDbContext context, UserManager<User> userManager,
            RoleManager<Role> roleManager, IHttpContextAccessor httpAccessor)
        {
            _context = context;

            _context.CurrentUserId = httpAccessor.HttpContext?.User
                .FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();

            _userManager = userManager;

            _roleManager = roleManager;
        }


        #region UserManage

        public async Task<Tuple<bool, string[]>> CreateUserAsync(User user, IEnumerable<string> roles, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            user = await _userManager.FindByNameAsync(user.UserName);

            try
            {
                result = await _userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<Tuple<User, string[]>> GetUserAndRolesAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();

            return Tuple.Create(user, roles);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<Tuple<User, string[]>>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            IQueryable<User> usersQuery = _context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);

            if (page != -1)
                usersQuery = usersQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                usersQuery = usersQuery.Take(pageSize);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users.Select(u => Tuple.Create(u,
                    roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
                .ToList();
        }

        #endregion

        #region RoleManage

        public async Task<Tuple<bool, string[]>> CreateRoleAsync(Role role, IEnumerable<string> claims)
        {
            if (claims == null)
                claims = new string[] { };

            var enumerable = claims as string[] ?? claims.ToArray();
            var invalidClaims = enumerable.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            if (invalidClaims.Any())
                return Tuple.Create(false,
                    new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });


            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            role = await _roleManager.FindByNameAsync(role.Name);

            foreach (var claim in enumerable.Distinct())
            {
                result = await _roleManager.AddClaimAsync(role,
                    new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<Role> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .Where(r => r.Name == roleName)
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<Role> rolesQuery = _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }

        #endregion
    }
}