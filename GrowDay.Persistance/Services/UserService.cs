using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class UserService : IUserService
    {
        protected readonly ILogger<UserService> _logger;
        protected readonly IReadUserRepository _readUserRepository;
        protected readonly INotificationService _notificationService;
        protected readonly IWriteNotificationRepository _writeNotificationRepository;
        protected readonly IWriteUserRepository _writeUserRepository;
        public UserService(ILogger<UserService> logger, IReadUserRepository readUserRepository, IWriteUserRepository writeUserRepository, INotificationService notificationService, IWriteNotificationRepository writeNotificationRepository)
        {
            _logger = logger;
            _readUserRepository = readUserRepository;
            _writeUserRepository = writeUserRepository;
            _notificationService = notificationService;
            _writeNotificationRepository = writeNotificationRepository;
        }
        public async Task<ICollection<Result<UserDTO>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _readUserRepository.GetAllAsync();
                var userDTOs = users!.Select(user => new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!
                }).ToList();
                var resultList = userDTOs.Select(dto => Result<UserDTO>.SuccessResult(dto)).ToList();
                return resultList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                return Array.Empty<Result<UserDTO>>();
            }
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _readUserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result<UserDTO>.FailureResult("User not found.");
                }
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!
                };
                return Result<UserDTO>.SuccessResult(userDTO, "User retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user by ID: {UserId}", userId);
                return Result<UserDTO>.FailureResult("An error occurred while retrieving the user.");
            }
        }

        public async Task<Result> RemoveUserAsync(string userId)
        {
            try
            {
                var user = await _readUserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result.FailureResult("User not found.");
                }
                await _writeNotificationRepository.RemoveRangeAsync(user.Notifications!);
                await _writeUserRepository.DeleteAsync(user);
                return Result.SuccessResult("User removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing user with ID: {UserId}", userId);
                return Result.FailureResult("An error occurred while removing the user.");
            }
        }
    }
}
