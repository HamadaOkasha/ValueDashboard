using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<AspNetRoles> GetItemById(string id);
        Task<bool> CheckExisting(string id, string roleName);
        Task<Tuple<IList<AspNetRoles>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null);
        Task SaveItem(AspNetRoles obj);
        Task SaveGroupPages(List<RolePages> list, string groupId);
        Task<IList<AdminPages>> GeAdminPagesList();
        Task<IList<Country>> GeCountryList();
        Task<IList<AspNetRoles>> GeRolesList();
        Task<bool> DeleteItem(string Id);
    }
}
