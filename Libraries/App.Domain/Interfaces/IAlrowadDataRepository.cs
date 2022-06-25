using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IAlrowadDataRepository
    {
        Task<bool> ChangeStatusItem(int id, bool status);
        Task<bool> DeleteItem(int id);
        Task<AlrowadData>  GetItemById(int id);  
        Task<Tuple<IList<AlrowadData>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0);
        Task SaveItem(AlrowadData obj);
    }
}
