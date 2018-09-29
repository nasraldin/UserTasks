using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Data.EntityConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired();

            builder.HasMany(r => r.Claims)
                .WithOne().HasForeignKey(c => c.RoleId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Users)
                .WithOne().HasForeignKey(r => r.RoleId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
