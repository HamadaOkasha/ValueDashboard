using App.Domain.Model;
using App.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class AdminPagesModel : BaseEntity, ILocalizedModel<AdminPagesLocalizedModel>
    {
        public AdminPagesModel()
        {
            Locales = new List<AdminPagesLocalizedModel>();
        }
        public string PageUrl { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string PageTitle { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public int? DisplayOrder { get; set; }
        public int? ParentId { get; set; }
        public SelectList ParentList { get; set; }
        public IList<AdminPagesLocalizedModel> Locales { get; set; }
    }
    public partial class AdminPagesLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        public string PageTitle { get; set; }
    }
}
