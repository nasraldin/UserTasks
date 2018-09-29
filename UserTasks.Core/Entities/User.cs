
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace UserTasks.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<int>> Roles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }

        /// <summary>
        /// Navigation property for the Tasks this user possesses.
        /// </summary>
        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}
