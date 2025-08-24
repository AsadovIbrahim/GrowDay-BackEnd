using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteNotificationRepository : WriteGenericRepository<Notification>, IWriteNotificationRepository
    {
        public WriteNotificationRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
