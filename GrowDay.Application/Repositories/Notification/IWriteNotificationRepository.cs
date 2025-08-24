using GrowDay.Domain.Entities.Concretes;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Application.Repositories
{
    public interface IWriteNotificationRepository: IWriteGenericRepository<Notification>
    {
    }
}
