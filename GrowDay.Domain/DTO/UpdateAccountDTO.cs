using System.ComponentModel.DataAnnotations;

namespace GrowDay.Domain.DTO
{
    public class UpdateAccountDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
