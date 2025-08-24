using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class ReadStatisticRepository : ReadGenericRepository<Statistic>, IReadStatisticRepository
    {
        public ReadStatisticRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
