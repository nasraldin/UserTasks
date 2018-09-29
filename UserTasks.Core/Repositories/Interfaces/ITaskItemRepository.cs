using System.Collections.Generic;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Repositories.Interfaces
{
    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        void CreateTask(TaskItem task);

        void UpdateTask(TaskItem taskItem);

        TaskItem GetTask(int id);

        IEnumerable<TaskItem> GetAllTasks();

        IEnumerable<TaskItem> GetAllTasksByUserId(int userId);

        void DeleteTask(int id);
    }
}
