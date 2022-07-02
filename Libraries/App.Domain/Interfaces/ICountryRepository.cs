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
        int LoadItemsDataCategory(int? CountryId);
        int LoadItemsDataCountry(int? CountryId, int? SectorId);
        int LoadItemsDataOrganization(int? CountryId, int? SectorId);
        int LoadItemsDataSector(int? CountryId, int? SectorId);
        int LoadItemsDataOrganzationWithValue(int? CountryId, int? SectorId);
        int LoadItemsDataOrganzationWithoutValue(int? CountryId, int? SectorId);
        int LoadItemsDataValue(int? CountryId, int? SectorId);

        //IList<TopTenViewModel> LoadItemsDataValueAndName();
    }
}
