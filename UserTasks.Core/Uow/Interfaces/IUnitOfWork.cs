using UserTasks.Core.Repositories.Interfaces;

namespace UserTasks.Core.Uow.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskItemRepository TaskItems { get; }

        int SaveChanges();
    }
}
