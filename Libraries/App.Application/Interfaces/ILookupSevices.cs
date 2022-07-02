using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Application.ViewModels;
using App.Application.Model;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface ILookupSevices
    {
        Task<IList<BaseViewModel>> GetParentList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetCountryList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetCategoryList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetSectorList(string selectTitle = null, int CountryId = 0);
        Task<IList<BaseViewModel>> GetOrganizationList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetValueList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetResearchValueList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetVersionList(string selectTitle = null);
        Task<IList<BaseViewModel>> GetValueAndCountList(string selectTitle = null, int CountryId=0);
        Task<IList<BaseViewModel>> GetCountryAndCountList(string selectTitle = null, int Index=0);

    }
}