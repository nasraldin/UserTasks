using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UserTasks.Core.Data;
using UserTasks.Core.Repositories.Interfaces;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        // ReSharper disable once InconsistentNaming
        protected readonly AppDbContext _context;
        //private readonly DbSet<TEntity> _entities;

        public Repository(AppDbContext context)
        {
            _context = context;
            // _entities = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            //return _entities.ToList();
            return _context.Set<TEntity>().ToList();
        }

        public virtual TEntity GetById(int id)
        {
            //return _entities.Find(id);
            return _context.Set<TEntity>().SingleOrDefault(e => e.Id == id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            //_entities.Add(entity);
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            //_entities.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            //_entities.Remove(entity);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            //_entities.RemoveRange(entities);
            _context.Set<TEntity>().RemoveRange(entities);
            _context.SaveChanges();
        }

        //public virtual int Count()
        //{
        //    return _entities.Count();
        //}

        //public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return _entities.Where(predicate);
        //}

        //public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return _entities.SingleOrDefault(predicate);
        //}
    }
}
