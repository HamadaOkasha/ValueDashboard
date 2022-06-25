using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Domain.Interfaces;
using System.Threading.Tasks;
using App.Application.ViewModels;

namespace App.Application.Services
{

    public class InitalizeModelLookups : IInitalizeModelLookups
    {
        private readonly ILookupSevices _lookupSevices;
        private readonly IGroupService _groupService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;


        public InitalizeModelLookups(ILookupSevices lookupSevices, ILocaleStringResourceSevices localeStringResourceSevices,
            IHttpContextAccessor httpContextAccessor,
             IGroupService groupService,
             IUserRepository userRepository)
        {
            _lookupSevices = lookupSevices;
            _groupService = groupService;
            _httpContextAccessor = httpContextAccessor;
            _localeStringResourceSevices = localeStringResourceSevices;
        }

        public async Task InitModel(AdminPagesModel model, string selectTitle = null)
        {
            model.ParentList = new SelectList(await _lookupSevices.GetParentList(selectTitle), "Id", "Name");
        }
        public async Task InitModel(GroupModel model, string selectTitle = null)
        {
            model.AdminPagesList = new SelectList(await _groupService.GeAdminPagesList(selectTitle), "Id", "Name");
        }
        public async Task InitModel(UserModel model, string selectTitle = null)
        {
            model.RolesList = new SelectList(await _groupService.GeRolesList(selectTitle), "IdString", "Name");
        }
        public async Task InitModel(AlrowadDataModel model, string selectTitle = null)
        {
            model.CountryList = new SelectList(await _lookupSevices.GetCountryList(selectTitle), "Id", "Name");
            model.CategoryList = new SelectList(await _lookupSevices.GetCategoryList(selectTitle), "Id", "Name");
            model.SectorList = new SelectList(await _lookupSevices.GetSectorList(selectTitle), "Id", "Name");
            model.OrganizationList = new SelectList(await _lookupSevices.GetOrganizationList(selectTitle), "Id", "Name");
            model.ValueList = new SelectList(await _lookupSevices.GetValueList(selectTitle), "Id", "Name");
            model.ResearchValueList = new SelectList(await _lookupSevices.GetResearchValueList(selectTitle), "Id", "Name");
            model.VersionList = new SelectList(await _lookupSevices.GetVersionList(selectTitle), "Id", "Name");
        }
        public async Task InitModel(HomeViewModel model, string selectTitle = null)
        {
            model.CountryList = new SelectList(await _lookupSevices.GetCountryList(selectTitle), "Id", "Name");
            model.SectorList = new SelectList(await _lookupSevices.GetSectorList(selectTitle), "Id", "Name");
            
        }
    }
}