"use strict";
// "View Users" | "Manage Users" | "View Roles" | "Manage Roles" | "Assign Roles" |
//"users.view" | "users.manage" | "roles.view" | "roles.manage" | "roles.assign" |
Object.defineProperty(exports, "__esModule", { value: true });
var Permission = /** @class */ (function () {
    //public static readonly viewUsersPermission: PermissionValues = "users.view";
    //public static readonly manageUsersPermission: PermissionValues = "users.manage";
    //public static readonly viewRolesPermission: PermissionValues = "roles.view";
    //public static readonly manageRolesPermission: PermissionValues = "roles.manage";
    //public static readonly assignRolesPermission: PermissionValues = "roles.assign";
    function Permission(name, value, groupName, description) {
        this.name = name;
        this.value = value;
        this.groupName = groupName;
        this.description = description;
    }
    Permission.viewTasksPermission = "tasks.view";
    Permission.manageTasksPermission = "tasks.manage";
    Permission.assignTasksPermission = "tasks.assign";
    Permission.adminTasksPermission = "tasks.listwithdelete";
    return Permission;
}());
exports.Permission = Permission;
//# sourceMappingURL=permission.model.js.map