using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserPreferencesService
    {
        Task<Result<UserPreferenceDTO>> GetUserPreferencesAsync(string userId);
        Task<Result<UserPreferenceDTO>> CreateUserPreferencesAsync(string userId, CreateUserPreferenceDTO createUserPreferenceDTO);
        Task<Result<UserPreferenceDTO>> UpdateUserPreferencesAsync(string userId, UpdateUserPreferenceDTO updateUserPreferenceDTO);
    }
}
