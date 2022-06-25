using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "FieldRequired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
    public class CustomerTokenModel
    {
        public string Token { get; set; }
    }
}
