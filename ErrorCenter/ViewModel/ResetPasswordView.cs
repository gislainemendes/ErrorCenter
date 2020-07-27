using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Central_De_Erros.ViewModel
{
    public class ResetPasswordView
    { 

        [Required(ErrorMessage = "The field {0} must be entered")]
        [EmailAddress(ErrorMessage = "The format is invalid for field {0}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        public string Token { get; set; }

        [Required(ErrorMessage = "The field {0} must be entered")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords are differents")]
        public string ConfirmThePassword { get; set; }
    }
}
