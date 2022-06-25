using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Application.Model;
using App.Application.ViewModels;
using System.Threading.Tasks;
using System;

namespace App.Application.Interfaces
{
    public interface IAdminPagesSevices
    {
        Task<AdminPagesModel> GetById(int id);
        Task Save(AdminPagesModel model);
        Task<Tuple<IList<AdminPagesViewModel>, int>> LoadData(string Search = null,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);
    }
}