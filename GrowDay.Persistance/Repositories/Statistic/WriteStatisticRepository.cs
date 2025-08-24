using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteStatisticRepository : WriteGenericRepository<Statistic>, IWriteStatisticRepository
    {
        public WriteStatisticRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
