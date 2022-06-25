using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IAdminPagesRepository
    {
        Task<AdminPages> GetItemById(int id);
        Task SaveItem(AdminPages model);
        Task<Tuple<IList<AdminPages>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
         int jtPageSize = 10, string order = null, string orderDir = null);
    }
}
