using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UserTasks.Core.Authorization
{
    public class ApplicationPermission
    {
        public ApplicationPermission() { }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }


        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }


        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }

    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        // Task Permissions
        public const string TasksPermissionGroupName = "Task Permissions";

        public static ApplicationPermission ViewTasks = new ApplicationPermission("View Tasks", "tasks.view", TasksPermissionGroupName, "Permission to view available tasks");

        public static ApplicationPermission ManageTasks = new ApplicationPermission("Manage Tasks", "tasks.manage", TasksPermissionGroupName, "Permission to create, delete and update tasks");

        public static ApplicationPermission AssignTasks = new ApplicationPermission("Assign Tasks", "tasks.assign", TasksPermissionGroupName, "Permission to assign task to another user");

        public static ApplicationPermission AdminTasks = new ApplicationPermission("Admin Tasks", "tasks.listwithdelete", TasksPermissionGroupName, "Permission to view all tasks and delete permission");

        // User Permissions
        public const string UsersPermissionGroupName = "User Permissions";

        public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view", UsersPermissionGroupName, "Permission to view other users account details");

        public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");

        // Role Permissions
        public const string RolesPermissionGroupName = "Role Permissions";

        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");

        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");

        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        static ApplicationPermissions()
        {
            var allPermissions = new List<ApplicationPermission>()
            {
                ViewTasks,
                ManageTasks,
                AssignTasks,

                ViewUsers,
                ManageUsers,

                ViewRoles,
                ManageRoles,
                AssignRoles
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.FirstOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.FirstOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string[] GetAdminPermissionValues()
        {
            return new string[] { ViewTasks, AdminTasks };
        }
    }
}
