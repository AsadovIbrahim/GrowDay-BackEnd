using GrowDay.Domain.Entities.Abstracts;

namespace GrowDay.Application.Repositories.Common
{
    public interface IWriteGenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity, new()
    {
        Task SaveChangesAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task DeleteByIdAsync(string id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
