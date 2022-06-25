using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Model
{
    public class Language : BaseEntity , ILocalizedEntity
    {
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public bool Rtl { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
