using System;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Interfaces
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// Related properties automatically set when saving/updating <see cref="BaseEntity"/> objects.
    /// </summary>
    public interface IAuditedEntity
    {
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
