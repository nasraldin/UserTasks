using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using UserTasks.Core;
using UserTasks.Core.Authorization;
using UserTasks.Helpers;

namespace UserTasks.Authorization
{
    public class UserAccountAuthRequirement : IAuthorizationRequirement
    {
        public UserAccountAuthRequirement(string operationName)
        {
            this.OperationName = operationName;
        }

        public string OperationName { get; private set; }
    }

    public class ViewUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthRequirement requirement, string targetUserId)
        {
            if (context.User == null || requirement.OperationName != AccountManagementOperations.ReadOperationName)
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers) || GetIsSameUser(context.User, targetUserId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private static bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
        {
            if (string.IsNullOrWhiteSpace(targetUserId))
                return false;

            return Utilities.GetUserId(user) == targetUserId;
        }
    }

    public class ManageUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthRequirement requirement, string targetUserId)
        {
            if (context.User == null ||
                (requirement.OperationName != AccountManagementOperations.CreateOperationName &&
                 requirement.OperationName != AccountManagementOperations.UpdateOperationName &&
                 requirement.OperationName != AccountManagementOperations.DeleteOperationName))
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers) || GetIsSameUser(context.User, targetUserId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }


        public bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
        {
            if (string.IsNullOrWhiteSpace(targetUserId))
                return false;

            return Utilities.GetUserId(user) == targetUserId;
        }
    }
}
