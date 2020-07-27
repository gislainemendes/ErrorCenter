using System.ComponentModel.DataAnnotations;

namespace Central_De_Erros.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "The field {0} must be entered")]
        [StringLength(80, ErrorMessage = "The field {0} must has between {2} and {1} caracters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        [StringLength(80, ErrorMessage = "The field {0} must has between {2} and {1} caracters", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        [EmailAddress(ErrorMessage = "The format is invalid for field {0}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords are differents")]
        public string ConfirmThePassword { get; set; }
    }
}
