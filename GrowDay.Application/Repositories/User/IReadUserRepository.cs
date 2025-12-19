using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserRepository:IReadGenericRepository<User>
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?>GetUserByUsername(string username);
        Task<User?> GetUserById(string id);
    }
}
