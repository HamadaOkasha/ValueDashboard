using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IGroupService
    {
        Task<GroupModel> GetById(string id);
        Task<bool> Save(GroupModel model);
        Task<Tuple<IList<GroupViewModel>, int>> LoadData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null);
        Task<IList<BaseViewModel>> GeAdminPagesList(string selectTitle = null);
        Task<IList<BaseViewModel>> GeCategoryList(string selectTitle = null);
        Task<IList<BaseViewModel>> GeRolesList(string selectTitle = null);
        Task<bool> Delete(string id);
    }
}
