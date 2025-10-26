using GrowDay.Domain.Entities.Concretes;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserActivityRepository:IReadGenericRepository<UserActivity>
    {
        Task<IEnumerable<UserActivity>>GetUserActivitiesAsync(string userId,int pageIndex=0,int pageSize=10);
    }
}
