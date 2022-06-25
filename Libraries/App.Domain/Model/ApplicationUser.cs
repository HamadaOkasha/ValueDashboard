using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public int UnitNumber { get; set; }
        public string Adress { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
