using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            RolePages = new HashSet<RolePages>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsMembers { get; set; }

        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<RolePages> RolePages { get; set; }
    }
}
