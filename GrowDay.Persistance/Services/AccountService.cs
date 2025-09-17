using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using GrowDay.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class AccountService : IAccountService
    {
        protected readonly ILogger<AccountService> _logger;
        protected readonly ITokenService _tokenService;
        protected readonly IAuthService _authService;
        protected readonly UserManager<User> _userManager;
        protected readonly IReadUserRepository _readUserRepository;
        protected readonly IWriteUserRepository _writeUserRepository;
        protected readonly IWriteUserTokenRepository _writeUserTokenRepository;
        protected readonly IReadNotificationRepository _readNotificationRepository;
        protected readonly IWriteNotificationRepository _writeNotificationRepository;
        public AccountService(ILogger<AccountService> logger, ITokenService tokenService, IAuthService authService, UserManager<User> userManager,
            IReadUserRepository readUserRepository, IWriteUserRepository writeUserRepository, IWriteUserTokenRepository writeUserTokenRepository,
            IReadNotificationRepository readNotificationRepository, IWriteNotificationRepository writeNotificationRepository)
        {
            _logger = logger;
            _tokenService = tokenService;
            _authService = authService;
            _userManager = userManager;
            _readUserRepository = readUserRepository;
            _writeUserRepository = writeUserRepository;
            _writeUserTokenRepository = writeUserTokenRepository;
            _readNotificationRepository = readNotificationRepository;
            _writeNotificationRepository = writeNotificationRepository;
        }

        public async Task<Result<int>> DeleteAccount(string username)
        {
            try
            {
                var user = await _readUserRepository.GetUserByUsername(username);
                if (user == null)
                {
                    return Result<int>.FailureResult("User not found.");
                }
                var notifications = await _readNotificationRepository.GetNotificationsByUserIdAsync(user.Id);
                foreach (var notification in notifications)
                {
                    await _writeNotificationRepository.DeleteAsync(notification);
                }

                var result= await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<int>.FailureResult($"Account deletion failed: {errors}");
                }
                return Result<int>.SuccessResult(1, "User account deleted successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting account for username: {Username}", username);
                return Result<int>.FailureResult("An error occurred while processing your request.");
            }
        }

        public async Task<Result<GetAccountData>> GetAccountData(string username)
        {
            try
            {
                var user = await _readUserRepository.GetUserByUsername(username);
                if (user == null)
                {
                    return Result<GetAccountData>.FailureResult("User not found.");
                }
                var accountData = new GetAccountData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Username = user.UserName!
                };
                return Result<GetAccountData>.SuccessResult(accountData, "User account data retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting account data for username: {Username}", username);
                return Result<GetAccountData>.FailureResult("An error occurred while processing your request.");
            }
        }

        public async Task<Result<int>> UpdateAccount(string username, UpdateAccountDTO updateAccountDTO, HttpResponse httpResponse)
        {
            try
            {
                var user = await _readUserRepository.GetUserByUsername(username);
                if (user == null)
                {
                    return Result<int>.FailureResult("User not found.");
                }
                if(!string.IsNullOrEmpty(updateAccountDTO.Email) && user.Email != updateAccountDTO.Email)
                {
                    var emailExists = await _readUserRepository.GetUserByEmail(updateAccountDTO.Email);
                    if (emailExists != null)
                    {
                        return Result<int>.FailureResult("Email is already in use.");
                    }
                }
                if (!string.IsNullOrEmpty(updateAccountDTO.Username) && user.UserName != updateAccountDTO.Username)
                {
                    var usernameExists = await _readUserRepository.GetUserByUsername(updateAccountDTO.Username);
                    if (usernameExists != null)
                    {
                        return Result<int>.FailureResult("Username is already in use.");
                    }
                }
                user.FirstName = updateAccountDTO.FirstName;
                user.LastName = updateAccountDTO.LastName;
                user.Email = updateAccountDTO.Email;

                if (user.UserName != updateAccountDTO.Username)
                {
                    user.UserName = updateAccountDTO.Username;
                    await _userManager.UpdateNormalizedUserNameAsync(user);

                    var refreshToken = _tokenService.CreateRefreshToken();
                    await _authService.SetRefreshToken(user, refreshToken, httpResponse);
                }
                if (!string.IsNullOrEmpty(updateAccountDTO.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, updateAccountDTO.Password);
                    if (!passwordResult.Succeeded)
                    {
                        var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                        return Result<int>.FailureResult($"Password update failed: {errors}");
                    }
                }
                await _writeUserRepository.UpdateAsync(user);
                return Result<int>.SuccessResult(1, "User account updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating account for username: {Username}", username);
                return Result<int>.FailureResult("An error occurred while processing your request.");
            }
        }
    }
}
