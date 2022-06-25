using App.Application.Model;
using App.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface ISettingsServices
    {
        Task<MailSettingModel> LoadMailSetting();
        Task SaveMailSetting(MailSettingModel Obj);
      
    }
}