using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace App.Domain.Interfaces
{
    public interface IAlrowadVersionRepository
    {
        Task<bool> ChangeStatusItem(int id, bool status);
        Task<bool> DeleteItem(int id);
        Task<AlrowadVersion>  GetItemById(int id);  
        Task<Tuple<IList<AlrowadVersion>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0);
        Task SaveItem(AlrowadVersion obj);
    }
}
