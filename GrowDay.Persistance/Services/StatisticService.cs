using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class StatisticService : IStatisticService
    {
        protected readonly IReadHabitRecordRepository _readHabitRecordRepository;
        protected readonly ILogger<StatisticService> _logger;
        public StatisticService(IReadHabitRecordRepository readHabitRecordRepository, ILogger<StatisticService> logger)
        {
            _readHabitRecordRepository = readHabitRecordRepository;
            _logger = logger;
        }

        public async Task<Result<StatisticDTO>> CalculateStatisticsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var habitRecords = await _readHabitRecordRepository.GetAllByUserAndDateRangeAsync(userId,startDate,endDate);
                
                var filteredRecords = habitRecords?
                    .Where(hr => hr.Date >= startDate && hr.Date <= endDate && !hr.IsDeleted)
                    .ToList() ?? new List<GrowDay.Domain.Entities.Concretes.HabitRecord>();

                int completedCount = filteredRecords.Count(hr => hr.IsCompleted);
                int missedCount = filteredRecords.Count(hr => !hr.IsCompleted);
                double completionRate = (completedCount + missedCount) > 0
                    ? (double)completedCount / (completedCount + missedCount) * 100
                    : 0;
                double missedRate= (completedCount + missedCount) > 0
                    ? (double)missedCount / (completedCount + missedCount) * 100
                    : 0;

                var statisticDTO = new StatisticDTO
                {
                    CompletedCount = completedCount,
                    MissedCount = missedCount,
                    PeriodStart = startDate,
                    PeriodEnd = endDate,
                    PeriodType = StatisticPeriodType.Custom,
                    CompletionRate=completionRate,
                    MissedRate= missedRate

                };

                return Result<StatisticDTO>.SuccessResult(statisticDTO, "Statistics calculated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while calculating statistics");
                return Result<StatisticDTO>.FailureResult("An error occurred while calculating statistics.");
            }
        }

        public async Task<Result<StatisticDTO>> GetMonthlyStatisticsAsync(string userId, int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var result = await CalculateStatisticsAsync(userId, startDate, endDate);
                if (result.Success && result.Data != null)
                {
                    result.Data.PeriodType = StatisticPeriodType.Monthly;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting monthly statistics");
                return Result<StatisticDTO>.FailureResult("An error occurred while getting monthly statistics.");
            }
        }

        public async Task<Result<StatisticDTO>> GetWeeklyStatisticAsync(string userId, DateTime? weekStart = null)
        {
            try
            {
                DateTime startDate;
                if (weekStart.HasValue)
                {
                    startDate = weekStart.Value.Date;
                }
                else
                {
                    var today = DateTime.UtcNow.Date;
                    int daysUntilMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                    startDate = today.AddDays(-daysUntilMonday);
                }
                
                var endDate = startDate.AddDays(6);
                var result = await CalculateStatisticsAsync(userId, startDate, endDate);
                if (result.Success && result.Data != null)
                {
                    result.Data.PeriodType = StatisticPeriodType.Weekly;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting weekly statistics");
                return Result<StatisticDTO>.FailureResult("An error occurred while getting weekly statistics.");
            }
        }

        public async Task<Result<StatisticDTO>> GetDailyStatisticAsync(string userId, DateTime? date = null)
        {
            try
            {
                var targetDate = date.HasValue ? date.Value.Date : DateTime.UtcNow.Date;
                var result = await CalculateStatisticsAsync(userId, targetDate, targetDate);
                if (result.Success && result.Data != null)
                {
                    result.Data.PeriodType = StatisticPeriodType.Daily;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting daily statistics");
                return Result<StatisticDTO>.FailureResult("An error occurred while getting daily statistics.");
            }
        }
    }
}
