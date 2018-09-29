using System.Threading.Tasks;

namespace UserTasks.Core.Data.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }
}
