using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using UserTasks.Core;
using UserTasks.Core.Authorization;

namespace UserTasks.Authorization
{
    public class AuthorizationRequirement : IAuthorizationRequirement { }

    public class AssignRolesAuthorizationHandler :
        AuthorizationHandler<AuthorizationRequirement, Tuple<string[], string[]>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, Tuple<string[], string[]> newAndCurrentRoles)
        {
            if (!GetIsRolesChanged(newAndCurrentRoles.Item1, newAndCurrentRoles.Item2))
            {
                context.Succeed(requirement);
            }
            else if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.AssignRoles))
            {
                // If user has ViewRoles permission, then he can assign any roles
                if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles))
                    context.Succeed(requirement);
                // Else user can only assign roles they're part of
                else if (GetIsUserInAllAddedRoles(context.User, newAndCurrentRoles.Item1, newAndCurrentRoles.Item2))
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


        private static bool GetIsRolesChanged(string[] newRoles, string[] currentRoles)
        {
            if (newRoles == null)
                newRoles = new string[] { };

            if (currentRoles == null)
                currentRoles = new string[] { };

            bool roleAdded = newRoles.Except(currentRoles).Any();
            bool roleRemoved = currentRoles.Except(newRoles).Any();

            return roleAdded || roleRemoved;
        }


        private static bool GetIsUserInAllAddedRoles(IPrincipal contextUser, string[] newRoles, string[] currentRoles)
        {
            if (newRoles == null)
                newRoles = new string[] { };

            if (currentRoles == null)
                currentRoles = new string[] { };

            var addedRoles = newRoles.Except(currentRoles);

            return addedRoles.All(contextUser.IsInRole);
        }
    }
}
