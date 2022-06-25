using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "FieldRequired")]
        [EmailAddress(ErrorMessage = "EmailFaild")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "FieldRequired")]
        [StringLength(100, ErrorMessage = "PasswordWorng", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [Required(ErrorMessage = "FieldRequired")]
        [Compare("Password", ErrorMessage = "ConfirmPasswordWorng")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
