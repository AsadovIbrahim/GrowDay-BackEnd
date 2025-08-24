using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IStatisticService
    {
        Task<Result<StatisticDTO>>CalculateStatisticsAsync(string userId,DateTime startDate,DateTime endDate);
        Task<Result<StatisticDTO>>GetMonthlyStatisticsAsync(string userId,int year, int month);
        Task<Result<StatisticDTO>>GetWeeklyStatisticAsync(string userId, DateTime weekStart);
    }
}
