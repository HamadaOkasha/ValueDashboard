using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class LocaleStringResourceModel
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public int LanguageId { get; set; }
    }
}
