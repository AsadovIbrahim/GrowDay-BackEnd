using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteTaskRepository : WriteGenericRepository<TaskEntity>, IWriteTaskRepository
    {
        public WriteTaskRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
