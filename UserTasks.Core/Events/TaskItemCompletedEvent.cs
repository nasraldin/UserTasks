using UserTasks.Core.Entities;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Events
{
    public class TaskItemCompletedEvent : BaseDomainEvent
    {
        public TaskItem CompletedItem { get; set; }

        public TaskItemCompletedEvent(TaskItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}
