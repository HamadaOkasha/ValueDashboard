using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Model
{
    public partial class GeneralSetting : BaseEntity, ILocalizedEntity
    {
        public string SMTP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> Port { get; set; }
        public bool IsEnableSsl { get; set; }
    }
}
