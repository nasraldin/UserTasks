using System.Collections.Generic;
using System.Linq;
using UserTasks.Core.Data;
using UserTasks.Core.Entities;
using UserTasks.Core.Repositories.Interfaces;

namespace UserTasks.Core.Repositories
{
    public class TaskItemRepository : Repository<TaskItem>, ITaskItemRepository
    {
        // ReSharper disable once InconsistentNaming
        private AppDbContext _dbContext => _context;

        public TaskItemRepository(AppDbContext context) : base(context) { }


        public void CreateTask(TaskItem task)
        {
            _dbContext.TaskItems.Add(task);
        }

        public void UpdateTask(TaskItem taskItem)
        {
            var task = _dbContext.TaskItems.Find(taskItem);
            if (task != null)
            {
                _dbContext.TaskItems.Update(task);
            }
        }

        public TaskItem GetTask(int id)
        {
            var task = _dbContext.TaskItems.Find(id);
            return task;
        }

        public IEnumerable<TaskItem> GetAllTasks()
        {
            return _dbContext.TaskItems.ToList();
        }

        public IEnumerable<TaskItem> GetAllTasksByUserId(int userId)
        {
            var tasks = _dbContext.TaskItems.Where(u => u.UserOwnerId == userId);
            return tasks;
        }

        public void AssignTask(int id, int userId)
        {
            var task = _dbContext.TaskItems.Find(id);
            if (task != null)
            {
                task.UserOwnerId = userId;
                _dbContext.TaskItems.Update(task);
            }
        }

        public void TaskDone(int id)
        {
            var task = _dbContext.TaskItems.Find(id);
            if (task != null)
            {
                task.MarkComplete();
            }
        }

        public bool Any(int id)
        {
            return _dbContext.TaskItems.Any(t => t.Id == id);
        }

        public void DeleteTask(int id)
        {
            var task = _dbContext.TaskItems.Find(id);
            if (task != null)
            {
                _dbContext.TaskItems.Remove(task);
            }
        }
    }
}
