using Ardalis.GuardClauses;
using UserTasks.Core.Events;
using UserTasks.Core.Interfaces;

namespace UserTasks.Core.Services
{
    public class TaskItemService : IHandle<TaskItemCompletedEvent>
    {
        public void Handle(TaskItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));
        }
    }
}
