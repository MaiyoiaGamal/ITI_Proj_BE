using System.ComponentModel.DataAnnotations;

namespace proj2.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
         public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
    }
}
