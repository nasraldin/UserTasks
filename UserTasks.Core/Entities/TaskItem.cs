using UserTasks.Core.Events;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Task { get; set; }
        public bool IsDone { get; private set; }

        public int UserOwnerId { get; set; }

        /// <summary>
        /// Navigation property for the user owner in this task.
        /// </summary>
        public User UserOwner { get; set; }


        public void MarkComplete()
        {
            IsDone = true;
            //Events.Add(new TaskItemCompletedEvent(this));
        }
    }
}
