using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using UserTasks.Core.Data;

namespace UserTasks
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Mapper.Reset();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("UserTasks"));
            builder.UseOpenIddict();

            return new AppDbContext(builder.Options);
        }
    }
}
