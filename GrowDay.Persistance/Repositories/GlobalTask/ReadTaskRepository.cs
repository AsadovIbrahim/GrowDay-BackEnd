using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class ReadTaskRepository : ReadGenericRepository<TaskEntity>, IReadTaskRepository
    {
        public ReadTaskRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
