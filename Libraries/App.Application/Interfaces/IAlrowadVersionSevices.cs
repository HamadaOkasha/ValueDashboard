using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Application.ViewModels;
 
using App.Application.Model;
using System.Threading.Tasks;
using System;
using App.Domain.Model;
namespace App.Application.Interfaces
{
    public interface IAlrowadVersionSevices
    {
        Task<AlrowadVersionModel>  GetById(int id);
        Task Save(AlrowadVersionModel model);
        Task<bool> ChangeStatus(int id, bool status);
          Task<bool> Delete(int id);
        Task<Tuple<IList<AlrowadVersionViewModel>, int>> LoadData(  string Search = null, int StatusId = 0 ,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);
        Task UpdateLocales(AlrowadVersion AlrowadVersion, AlrowadVersionModel model);
    }
}