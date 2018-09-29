namespace UserTasks.Core
{
    public static class CustomClaimTypes
    {
        ///<summary>A claim that specifies the permission of an entity</summary>
        public const string Permission = "permission";

        ///<summary>A claim that specifies the full name of an entity</summary>
        public const string FullName = "fullname";

        ///<summary>A claim that specifies the email of an entity</summary>
        public const string Email = "email";
    }

    public static class RoleName
    {
        /// <summary>
        /// Administrator Role
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Support Role
        /// </summary>
        public const string Support = "Support";
    }
}
