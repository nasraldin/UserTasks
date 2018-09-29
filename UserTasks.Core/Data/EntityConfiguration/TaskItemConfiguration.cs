using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Data.EntityConfiguration
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(t => t.Task).IsRequired();
        }
    }
}
