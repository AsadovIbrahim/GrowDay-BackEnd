using GrowDay.Application.Repositories;
using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserTaskCompletionRepository : WriteGenericRepository<UserTaskCompletion>, IWriteUserTaskCompletionRepository
    {
        public WriteUserTaskCompletionRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
