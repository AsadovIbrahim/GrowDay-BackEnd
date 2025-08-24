using GrowDay.Domain.Helpers;

namespace GrowDay.Domain.ViewModels
{
    public class LoginVM
    {
        public string? Error { get; set; }
        public List<string> Roles { get; set; }
        public TokenCredentials AccessToken { get; set; }
    }
}
