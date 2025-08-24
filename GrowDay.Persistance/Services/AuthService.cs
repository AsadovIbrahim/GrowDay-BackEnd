using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using GrowDay.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GrowDay.Persistance.Services
{
    public class AuthService : IAuthService
    {
        public HttpResponse Response { get; set; }
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IReadUserRepository _readUserRepository;
        private readonly IWriteUserRepository _writeUserRepository;
        private readonly IReadUserTokenRepository _readUserTokenRepository;
        private readonly IWriteUserTokenRepository _writeUserTokenRepository;
        private readonly INotificationService _notificationService;

        public AuthService(
            UserManager<User> userManager, SignInManager<User> signInManager,
            IReadUserRepository readUserRepository, IWriteUserRepository writeUserRepository, ITokenService tokenService,
            IEmailService emailService, IReadUserTokenRepository readUserTokenRepository, IWriteUserTokenRepository writeUserTokenRepository, 
            INotificationService notificationService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _signInManager = signInManager;
            _readUserRepository = readUserRepository;
            _writeUserRepository = writeUserRepository;
            _readUserTokenRepository = readUserTokenRepository;
            _writeUserTokenRepository = writeUserTokenRepository;
            _notificationService = notificationService;
        }

        public async Task<Result> RefreshLogin(string refreshToken)
        {
            var user = await _readUserTokenRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                return Result.FailureResult("Invalid refresh token.");

            await _signInManager.RefreshSignInAsync(user);
            return Result.SuccessResult("Login refreshed successfully.");
        }

        public async Task<Result<LoginVM>> Login(LoginDTO loginDTO, HttpResponse response)
        {
            if (Response == null) Response = response;

            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user is null)
                return Result<LoginVM>.FailureResult("Username is wrong.");

            if (!user.EmailConfirmed)
                return Result<LoginVM>.FailureResult("Email not confirmed.");

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
            if (!signInResult.Succeeded)
                return Result<LoginVM>.FailureResult("Password is wrong.");

            if(user.FirstTimeLogin)
            {
                user.FirstTimeLogin = false;
                await _writeUserRepository.UpdateAsync(user);
                await _writeUserRepository.SaveChangesAsync();
                await _notificationService.CreateAndSendNotificationAsync(null,
                    user.Id, 
                    "Welcome to GrowDay!", 
                    "We're excited to have you on board. Start your journey towards better habits today!");
            }
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var accesstoken = await _tokenService.CreateAccessToken(user);
            var refreshtoken = _tokenService.CreateRefreshToken();

            await SetRefreshToken(user, refreshtoken);

            var loginVM = new LoginVM
            {
                Roles = roles,
                AccessToken = accesstoken
            };

            return Result<LoginVM>.SuccessResult(loginVM, "Login successful.");
        }

        public async Task<Result> Register(RegisterDTO registerDTO, HttpResponse response)
        {
            if (Response == null) Response = response;

            var user = await _readUserRepository.GetUserByUsername(registerDTO.Username);
            if (user is not null)
                return Result.FailureResult("Username already exists.");

            user = await _readUserRepository.GetUserByEmail(registerDTO.Email);
            if (user is not null)
                return Result.FailureResult("Email already exists.");

            if (registerDTO.Password != registerDTO.ConfirmPassword)
                return Result.FailureResult("Passwords do not match.");

            var createResult = await CreateNewUser(registerDTO);
            return createResult.Success ? createResult : Result.FailureResult("User registration failed.");
        }

        public async Task<Result> SetRefreshToken(User user, TokenCredentials refreshToken, HttpResponse response = null)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.ExpireTime
            };

            if (response != null)
                Response = response;

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            var tokenstodelete = user.UserTokens.Where(p => p.Name == "RefreshToken" && !p.IsDeleted);
            foreach (var token in tokenstodelete)
                token.IsDeleted = true;

            var refreshUserToken = new UserToken()
            {
                UserId = user.Id,
                Name = "RefreshToken",
                Token = refreshToken.Token,
                ExpireTime = refreshToken.ExpireTime.AddHours(4)
            };
            await _writeUserTokenRepository.AddAsync(refreshUserToken);

            return Result.SuccessResult("Refresh token set successfully.");
        }

        public async Task<Result<TokenCredentials>> RefreshToken(HttpResponse response, HttpRequest request)
        {
            if (Response == null) Response = response;

            var refreshToken = request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Result<TokenCredentials>.FailureResult("No refresh token provided.");

            var user = await _readUserTokenRepository.GetUserByRefreshToken(refreshToken);
            if (user is null)
                return Result<TokenCredentials>.FailureResult("Invalid refresh token.");

            var accessToken = await _tokenService.CreateAccessToken(user);
            var refreshTokenObj = _tokenService.CreateRefreshToken();

            await _writeUserRepository.UpdateAsync(user);
            await SetRefreshToken(user, refreshTokenObj);

            return Result<TokenCredentials>.SuccessResult(accessToken, "Access token refreshed successfully.");
        }

        private async Task<Result> CreateNewUser(RegisterDTO registerDTO)
        {
            var newUser = new User()
            {
                FirstName = registerDTO.Firstname,
                LastName = registerDTO.Lastname,
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
            };

            var registerResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (registerResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(newUser.UserName);
                await _userManager.AddToRoleAsync(user!, "User");

                await SendConfirmEmail(registerDTO, newUser);
                return Result.SuccessResult("User registered successfully.");
            }
            return Result.FailureResult("User creation failed.");
        }

        private async Task SendConfirmEmail(RegisterDTO registerDTO, User newUser)
        {
            var confirmEmailToken = _tokenService.CreateConfirmEmailToken();
            var actionUrl = $@"https://localhost:7136/api/Auth/ConfirmEmail?token={confirmEmailToken.Token}";
            await _emailService.sendMailAsync(registerDTO.Email, "Confirm Your Email", $"Confirm your password by <a href='{actionUrl}'>clicking here</a>.", true);

            var userConfirmEmailToken = new UserToken()
            {
                UserId = newUser.Id,
                Name = "ConfirmEmailToken",
                Token = confirmEmailToken.Token,
                ExpireTime = confirmEmailToken.ExpireTime,
            };
            await _writeUserTokenRepository.AddAsync(userConfirmEmailToken);
        }

        public async Task<Result> ConfirmEmail(string token)
        {
            var user = await _readUserTokenRepository.GetUserByConfirmEmailToken(token);
            if (user is null)
                return Result.FailureResult("User not found.");

            var confirmEmailToken = await _readUserTokenRepository.GetConfirmEmailToken(token);
            if (confirmEmailToken.ExpireTime < DateTime.UtcNow)
            {
                confirmEmailToken.IsDeleted = true;
                return Result.FailureResult("Confirm email token expired.");
            }

            confirmEmailToken.IsDeleted = true;
            await _writeUserTokenRepository.UpdateAsync(confirmEmailToken);

            user.EmailConfirmed = true;
            await _writeUserRepository.UpdateAsync(user);

            return Result.SuccessResult("Email confirmed successfully.");
        }

        public async Task<Result> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _readUserRepository.GetUserByEmail(forgotPasswordDTO.Email);
            if (user is null)
                return Result.FailureResult("User not found.");

            var tokenstodelete = user.UserTokens!.Where(p => p.Name == "RepasswordToken" && !p.IsDeleted);
            foreach (var token in tokenstodelete)
                token.IsDeleted = true;

            var forgotPasswordToken = await _tokenService.CreateRepasswordToken(user);
            var actionUrl = $@"https://localhost:7136/api/resetpassword?token={forgotPasswordToken.Token}";
            await _emailService.sendMailAsync(forgotPasswordDTO.Email, "Reset Your Password", $"Reset your password by <a href='{actionUrl}'>clicking here</a>.", true);

            var userForgotPasswordToken = new UserToken()
            {
                UserId = user.Id,
                Name = "RepasswordToken",
                Token = forgotPasswordToken.Token,
                ExpireTime = forgotPasswordToken.ExpireTime,
            };
            await _writeUserTokenRepository.AddAsync(userForgotPasswordToken);

            return Result.SuccessResult("Password reset email sent.");
        }

        public async Task<Result> ResetPassword(string token, ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _readUserTokenRepository.GetUserByRePasswordToken(token);
            if (user is null)
                return Result.FailureResult("User not found.");

            var rePasswordToken = await _readUserTokenRepository.GetResetPasswordToken(token);
            if (rePasswordToken.ExpireTime < DateTime.UtcNow)
                return Result.FailureResult("Reset password token expired.");

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.Password);
            if (result.Succeeded)
            {
                rePasswordToken.IsDeleted = true;
                await _writeUserRepository.UpdateAsync(user);
                await _writeUserRepository.SaveChangesAsync();
                await _writeUserTokenRepository.UpdateAsync(rePasswordToken);
                return Result.SuccessResult("Password reset successfully.");
            }
            return Result.FailureResult("Password reset failed.");
        }
    }
}
