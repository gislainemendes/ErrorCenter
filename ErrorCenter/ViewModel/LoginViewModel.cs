using System.ComponentModel.DataAnnotations;

namespace Central_De_Erros.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The field {0} must be entered")]
        [EmailAddress(ErrorMessage = "The format is invalid for field {0}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        public string Password { get; set; }
    }
}
