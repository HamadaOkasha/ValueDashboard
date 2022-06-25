using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IGeneralSettingRepository
    {
       Task <GeneralSetting> GetItem(int Id);
        Task<GeneralSetting> GetCurrentItem();
        Task SaveItem(GeneralSetting obj);
    }
}
