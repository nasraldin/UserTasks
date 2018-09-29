using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using UserTasks.Core;
using UserTasks.Core.Authorization;

namespace UserTasks.Authorization
{
    public class ViewRoleAuthRequirement : IAuthorizationRequirement { }

    public class ViewRoleAuthorizationHandler : AuthorizationHandler<ViewRoleAuthRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewRoleAuthRequirement requirement, string roleName)
        {
            if (context.User == null)
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles) || context.User.IsInRole(roleName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
