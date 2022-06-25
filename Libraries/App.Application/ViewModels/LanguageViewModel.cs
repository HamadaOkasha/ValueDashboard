using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class LanguageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublish { get; set; }
    }
}
