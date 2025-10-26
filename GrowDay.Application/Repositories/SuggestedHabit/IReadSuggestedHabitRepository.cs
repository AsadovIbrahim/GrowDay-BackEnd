using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadSuggestedHabitRepository:IReadGenericRepository<SuggestedHabit>
    {
        Task<ICollection<SuggestedHabit>>GetSuggestedUserHabitsAsync(int pageIndex=0,int pageSize=10);
        Task<ICollection<SuggestedHabit>>GetSuggestedHabitsAsync(int pageIndex=0,int pageSize=10);
    }
}
