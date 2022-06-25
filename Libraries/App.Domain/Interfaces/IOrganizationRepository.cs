using App.Domain.Model;
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<bool> ChangeStatusItem(int id, bool status);
        Task<bool> DeleteItem(int id);
        Task<Organization>  GetItemById(int id);  
        //gg
        Task<Tuple<IList<Organization>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0);
        Task SaveItem(Organization obj);
    }
}
