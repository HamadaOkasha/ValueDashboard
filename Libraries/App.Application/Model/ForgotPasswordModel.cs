using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "FieldRequired")]
        [EmailAddress(ErrorMessage = "EmailFaild")]
        public string Email { get; set; }
    }
}
