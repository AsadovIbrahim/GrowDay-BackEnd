using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    internal class WriteUserTaskRepository : WriteGenericRepository<UserTask>, IWriteUserTaskRepository
    {
        public WriteUserTaskRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
