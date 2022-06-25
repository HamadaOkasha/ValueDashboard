using App.Domain.Model;
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<bool> ChangeStatusItem(int id, bool status);
        Task<bool> DeleteItem(int id);
        Task<Country>  GetItemById(int id);  
        Task<Tuple<IList<Country>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0);
        Task SaveItem(Country obj);
        int LoadItemsDataCategory(int? country);
        int LoadItemsDataCountry(int? country);
        int LoadItemsDataOrganization();
        int LoadItemsDataSector();
        int LoadItemsDataOrganzationWithValue();
        int LoadItemsDataOrganzationWithoutValue();
        int LoadItemsDataValue();

        //IList<TopTenViewModel> LoadItemsDataValueAndName();
    }
}
