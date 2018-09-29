using System.Collections.Generic;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        //int Count();

        //IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        //TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);
    }
}
