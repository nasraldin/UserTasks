using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}
