using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserService
    {
        Task<ICollection<Result<UserDTO>>> GetAllUsersAsync();
        Task<Result<UserDTO>> GetUserByIdAsync(string userId);
        Task<Result>RemoveUserAsync(string userId);
    }
}
