namespace UserTasks.Authorization
{
    public class Policies
    {
        ///<summary>
        /// Policy to allow viewing all user records.
        /// </summary>
        public const string ViewAllUsers = "View All Users";

        /// <summary>
        /// Policy to allow adding, removing and updating all user records.
        /// </summary>
        public const string ManageAllUsers = "Manage All Users";

        /// <summary>Policy to allow viewing details of all roles.</summary>
        public const string ViewAllRoles = "View All Roles";

        /// <summary>
        /// Policy to allow viewing details of all or specific roles (Requires roleName as parameter).
        /// </summary>
        public const string ViewRoleByRoleName = "View Role by RoleName";

        /// <summary>Policy to allow adding, removing and updating all roles.</summary>
        public const string ManageAllRoles = "Manage All Roles";

        /// <summary>
        /// Policy to allow assigning roles the user has access to (Requires new and current roles as parameter).
        /// </summary>
        public const string AssignAllowedRoles = "Assign Allowed Roles";
    }

    /// <summary>
    /// Operation Policy to allow adding, viewing, updating and deleting general or specific user records.
    /// </summary>
    public static class AccountManagementOperations
    {
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";

        public static UserAccountAuthRequirement Create = new UserAccountAuthRequirement(CreateOperationName);
        public static UserAccountAuthRequirement Read = new UserAccountAuthRequirement(ReadOperationName);
        public static UserAccountAuthRequirement Update = new UserAccountAuthRequirement(UpdateOperationName);
        public static UserAccountAuthRequirement Delete = new UserAccountAuthRequirement(DeleteOperationName);
    }
}
