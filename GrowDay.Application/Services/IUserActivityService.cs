using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserActivityService
    {
        Task<Result<UserActivityDTO>> CreateActivityAsync(CreateActivityDTO createActivityDTO);
        Task<Result<IEnumerable<UserActivityDTO>>> GetUserActivitiesAsync(string userId,int pageIndex=0,int pageSize=10);
        Task<Result> DeleteActivityAsync(string userId, string activityId);
        Task<Result> ClearActivityAsync(string userId);
        Task<Result<int>>GetUserTotalPointsAsync(string userId);

    }
}
