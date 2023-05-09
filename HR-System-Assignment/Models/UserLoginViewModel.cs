using System.ComponentModel.DataAnnotations;

namespace HR_System_Assignment.Models
{
    public class UserLoginViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
