"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Permission = /** @class */ (function () {
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