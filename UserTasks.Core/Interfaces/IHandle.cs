using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}
