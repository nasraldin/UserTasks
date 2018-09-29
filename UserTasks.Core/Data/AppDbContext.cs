using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserTasks.Core.Data.EntityConfiguration;
using UserTasks.Core.Entities;
using UserTasks.Core.Interfaces;
using UserTasks.Core.SharedKernel;

namespace UserTasks.Core.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IDomainEventDispatcher _dispatcher;

        /// <inheritdoc />
        /// <summary>
        /// AppDbContext ctor
        /// </summary>
        public AppDbContext(DbContextOptions options) : base(options) { }

        public AppDbContext(DbContextOptions options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// App DbSets
        /// </summary>
        public string CurrentUserId { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Specify DbSet properties etc..
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // load models configuration
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new TaskItemConfiguration());
        }

        #region OverrideSaveChanges

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            //return base.SaveChanges();

            var result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion

        /// <summary>
        /// Update Audit Entities
        /// </summary>
        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (BaseEntity)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
