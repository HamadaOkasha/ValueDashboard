using App.Domain.Interfaces;
using App.Domain.Model;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class AdminPages : BaseEntity, ILocalizedEntity
    {
        public AdminPages()
        {
            RolePages = new HashSet<RolePages>();
        }

        public string PageUrl { get; set; }
        public string PageTitle { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual ICollection<RolePages> RolePages { get; set; }
    }
}
