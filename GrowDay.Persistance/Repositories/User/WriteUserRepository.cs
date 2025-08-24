using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserRepository : WriteGenericRepository<User>, IWriteUserRepository
    {
        public WriteUserRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
