using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadHabitRecordRepository: IReadGenericRepository<HabitRecord>
    {
        Task<IEnumerable<HabitRecord>> GetAllByUserAsync(string userId);
        Task<IEnumerable<HabitRecord>> GetAllByHabitIdAsync(string habitId);
        Task<HabitRecord?> GetByUserHabitIdAndDateAsync(string userHabitId, DateTime date);
        Task<IEnumerable<HabitRecord>>GetAllByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<HabitRecord>>GetAllUserHabitIdAsync(string userHabitId);
    }
}
