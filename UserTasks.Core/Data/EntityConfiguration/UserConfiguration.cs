using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserTasks.Core.Entities;

namespace UserTasks.Core.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Email).IsRequired();

            builder.HasMany(u => u.Claims)
                .WithOne().HasForeignKey(c => c.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Roles)
                .WithOne().HasForeignKey(r => r.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
