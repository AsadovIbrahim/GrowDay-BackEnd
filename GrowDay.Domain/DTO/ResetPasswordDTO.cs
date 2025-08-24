using System.ComponentModel.DataAnnotations;

namespace GrowDay.Domain.DTO
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
