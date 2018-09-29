
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UserTasks.Core.Entities
{
    public class Role : IdentityRole<int>
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of <see cref="T:UserTasks.Core.Entities.Role" />.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Role()
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of <see cref="T:UserTasks.Core.Entities.Role" />.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        /// </remarks>
        public Role(string roleName) : base(roleName)
        {

        }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<IdentityUserRole<int>> Users { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<int>> Claims { get; set; }
    }
}
