using App.Application.Model;
using App.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IGeneralSettingSevices
    {
        Task<GeneralSettingModel> GetItem();
        Task Save(GeneralSettingModel model);
    }
}