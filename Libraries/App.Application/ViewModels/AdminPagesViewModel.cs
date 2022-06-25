using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class AdminPagesViewModel
    {
        public int Id { get; set; }
        public string PageTitle { get; set; }
        public string PageUrl { get; set; }
        public string ParentPageTitle { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
    }
}
