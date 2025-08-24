using GrowDay.Domain.Entities.Abstracts;

namespace GrowDay.Application.Repositories.Common
{
    public interface IGenericRepository<T> where T : class,IBaseEntity,new()
    {

    }
}
