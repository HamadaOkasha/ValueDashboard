using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface ILocaleStringResourceRepository
    {
        Task<LocaleStringResource> GetItemById(int id);
        Task<bool> Delete(int id);
        Task<IList<LocaleStringResource>> GetAllResource(int platformId = 1);
        Task<bool> CheckResourceName(string ResourceName,int currentId , int langId = 1);
        Task SaveItem(LocaleStringResource model);
        Task<Tuple<IList<LocaleStringResource>,int>> LoadItemsData(string ResourceName, string ResourceValue, int LanguageId, int jtStartIndex = 0,
         int jtPageSize = 10, string order = null, string orderDir = null);
        Task<IList<LocaleStringResource>> GetAllResourceMobile(int LanguageId = 1);
    }
}
