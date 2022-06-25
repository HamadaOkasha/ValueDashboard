using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class LanguageModel
    {
        public LanguageModel()
        {
            ResourceModel = new LocaleStringResourceModel();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public bool Rtl { get; set; }
        public bool IsPublish { get; set; }
        public int DisplayOrder { get; set; }

        public LocaleStringResourceModel ResourceModel { get; set; }
    }
}
