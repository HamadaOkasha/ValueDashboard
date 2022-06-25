using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class GeneralSettingModel : BaseModel
    {
        [Required(ErrorMessage = "FieldRequired")]
        public string SMTP { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string Password { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public Nullable<int> Port { get; set; }
        public bool IsEnableSsl { get; set; }
    }
}
