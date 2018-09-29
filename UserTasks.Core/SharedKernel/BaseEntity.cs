using System;
using System.Collections.Generic;
using UserTasks.Core.Interfaces;

namespace UserTasks.Core.SharedKernel
{
    /// <inheritdoc />
    /// <summary>
    /// An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    public abstract class BaseEntity : IAuditedEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Reference to the updated user of this entity.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
