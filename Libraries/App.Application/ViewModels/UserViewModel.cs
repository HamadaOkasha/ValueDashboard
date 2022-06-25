using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public int UnitNumber { get; set; }
        public bool IsMembers { get; set; }
        public bool IsPaymentAccess { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Adress { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
