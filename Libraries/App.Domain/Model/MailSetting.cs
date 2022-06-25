using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class MailSetting
    {
        public int Id { get; set; }
        public string Smtp { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Port { get; set; }
        public bool IsEnableSsl { get; set; }
    }
}
