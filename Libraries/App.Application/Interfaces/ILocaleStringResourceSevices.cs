using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Model;
using App.Application.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace App.Application.Interfaces
{
    public interface ILocaleStringResourceSevices
    {
        Task<LocaleStringResourceModel> GetById(int id);
        string GetResource(string resourceKey);
        Task<IList<LocalizedString>> GetAllResource();
        Task<bool> Delete(int id);
        Task<bool> Save(LocaleStringResourceModel model);
      Task<IList<LocalizedString>> GetAllResourceMobile(int languageId);
        Task<Tuple<IList<LocaleStringResourceViewModel>,int>> LoadData(string ResourceName = null, string ResourceValue = null,int languageId = 0,
            int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);
    }
}