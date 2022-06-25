using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class MessagesTemplateModel : BaseModel, ILocalizedModel<MessagesTemplateLocalizedModel>
    {
        public MessagesTemplateModel()
        {
            Locales = new List<MessagesTemplateLocalizedModel>();
        }
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public IList<MessagesTemplateLocalizedModel> Locales { get; set; }

    }
    public partial class MessagesTemplateLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
