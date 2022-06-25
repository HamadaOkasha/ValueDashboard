using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface ISettingsRepository
    {
        Task<MailSetting> LoadMailSetting();
        Task<bool> SaveMailSetting(MailSetting Obj);
    }
}
