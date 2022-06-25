using App.Domain.Model;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class RolePages
    {
        public int Id { get; set; }
        public int? PageId { get; set; }
        public string RoleId { get; set; }

        public virtual AdminPages AdminPages { get; set; }
        public virtual AspNetRoles AspNetRoles { get; set; }
    }
}
