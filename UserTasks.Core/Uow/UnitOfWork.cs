using UserTasks.Core.Data;
using UserTasks.Core.Repositories;
using UserTasks.Core.Repositories.Interfaces;
using UserTasks.Core.Uow.Interfaces;

namespace UserTasks.Core.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private ITaskItemRepository _taskItems;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITaskItemRepository TaskItems => _taskItems ?? (_taskItems = new TaskItemRepository(_context));

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
