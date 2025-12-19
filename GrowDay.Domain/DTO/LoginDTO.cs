using System.ComponentModel.DataAnnotations;

namespace GrowDay.Domain.DTO
{
    public class LoginDTO
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
