
export type PermissionNames = "View Tasks" | "Manage Tasks" | "Assign Tasks" | "Admin Tasks";

export type PermissionValues = "tasks.view" | "tasks.manage" | "tasks.assign" | "tasks.listwithdelete";

export class Permission {

  public static readonly viewTasksPermission: PermissionValues = "tasks.view";
  public static readonly manageTasksPermission: PermissionValues = "tasks.manage";
  public static readonly assignTasksPermission: PermissionValues = "tasks.assign";
  public static readonly adminTasksPermission: PermissionValues = "tasks.listwithdelete";


  constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
    this.name = name;
    this.value = value;
    this.groupName = groupName;
    this.description = description;
  }

  public name: PermissionNames;
  public value: PermissionValues;
  public groupName: string;
  public description: string;
}
