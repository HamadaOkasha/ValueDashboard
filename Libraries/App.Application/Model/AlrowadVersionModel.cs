using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class AlrowadVersionModel : BaseModel, ILocalizedModel<AlrowadVersionLocalizedModel>
    {
        public AlrowadVersionModel()
        {
            Locales = new List<AlrowadVersionLocalizedModel>();
        }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }

        public IList<AlrowadVersionLocalizedModel> Locales { get; set; }
    }
    public partial class AlrowadVersionLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
       
    }
}
