using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserPreferencesRepository : WriteGenericRepository<UserPreferences>, IWriteUserPreferencesRepository
    {
        public WriteUserPreferencesRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
