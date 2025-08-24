using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using GrowDay.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GrowDay.Application.Services
{
    public interface IAuthService
    {
        Task<Result> ConfirmEmail(string token);
        Task<Result> RefreshLogin(string refreshToken);
        Task<Result<LoginVM>> Login(LoginDTO loginDTO, HttpResponse response);
        Task<Result> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task<Result> SetRefreshToken(User user, TokenCredentials refreshToken, HttpResponse response = null);
        Task<Result> Register(RegisterDTO registerDTO, HttpResponse response);
        Task<Result> ResetPassword(string token, ResetPasswordDTO resetPasswordDTO);
        Task<Result<TokenCredentials?>> RefreshToken(HttpResponse response, HttpRequest request);
    }
}
