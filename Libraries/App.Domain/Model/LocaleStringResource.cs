using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class LocaleStringResource
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public int LanguageId { get; set; }
    }
}
