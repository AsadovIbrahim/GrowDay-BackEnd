using GrowDay.Application.Services;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GrowDay.Infrastructure.Services
{
    public class TokenService : ITokenService
    {

        // Fields

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        // Constructor

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Methods

        public async Task<TokenCredentials> CreateAccessToken(User user)
        {

            var result = await _userManager.GetRolesAsync(user);
            var roleName = String.Join(',', result);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),

                Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Email, user.Email!)
            })
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken? token = tokenHandler.CreateToken(tokenDescription);

            var accessToken = new TokenCredentials()
            {
                Token = tokenHandler.WriteToken(token),
                ExpireTime = DateTime.Now.AddMinutes(30)
            };

            return accessToken;
        }

        public TokenCredentials CreateRefreshToken()
        {

            var refreshToken = new TokenCredentials()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpireTime = DateTime.UtcNow.AddDays(7)
            };
            return refreshToken;
        }

        public async Task<TokenCredentials> CreateRepasswordToken(User user)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var repasswordToken = new TokenCredentials() { Token = token, ExpireTime = DateTime.UtcNow.AddMinutes(20) };
            return repasswordToken;
        }

        public TokenCredentials CreateConfirmEmailToken()
        {

            var confirmEmailToken = new TokenCredentials()
            {
                Token = Guid.NewGuid().ToString(),
                ExpireTime = DateTime.UtcNow.AddMinutes(30)
            };
            return confirmEmailToken;
        }
    }
}
