using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
 
    public class UserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "FieldRequired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        [EmailAddress(ErrorMessage = "EmailFaild")]
        public string Email { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        [StringLength(100, ErrorMessage = "PasswordWorng", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        [Compare("Password", ErrorMessage = "ConfirmPasswordWorng")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        [Range(0, int.MaxValue, ErrorMessage = "MobileNotCorrect")]
        //[RegularExpression(@"^[0][5]\d{8}", ErrorMessage = "MobileNotCorrect")]
        [MinLength(11, ErrorMessage = "MobileLength")]
        [MaxLength(11, ErrorMessage = "MobileLength")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public int UnitNumber { get; set; }
        public string Adress { get; set; }

        public string ReturnUrl { get; set; }
        public SelectList RolesList { get; set; }
        public int? WorkingLanguageId { get; set; }
    }


    
}
