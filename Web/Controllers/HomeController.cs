using App.Application.Interfaces;
using App.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICountrySevices _CountrySevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public HomeController(ILocaleStringResourceSevices localeStringResourceSevices,IInitalizeModelLookups initalizeModelLookups,ICountrySevices CountrySevices, ILogger<HomeController> logger)
        {
            _CountrySevices = CountrySevices;
            _logger = logger;
            _initalizeModelLookups = initalizeModelLookups;
            _localeStringResourceSevices = localeStringResourceSevices;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index(int CountryId=0, int SectorId = 0)
       {
            var model = new Models.HomeViewModel();
            var model1 = _CountrySevices.LoadDataTotal(CountryId, SectorId);
            var model2 = new App.Application.ViewModels.HomeViewModel();
            var model3 = new App.Application.ViewModels.HomeViewModel();
            model2.CountryId = CountryId;
            model2.SectorId = SectorId;
            await _initalizeModelLookups.InitModel(model2, _localeStringResourceSevices.GetResource("World Wide"));
            await _initalizeModelLookups.InitModel(model3, _localeStringResourceSevices.GetResource("World Wide"));
            model.TotalCountry = model1.TotalCountry;
            model.TotalSector = model1.TotalSector;
            model.TotalValue = model1.TotalValue;
            model.TotalOrganization = model1.TotalOrganization;
            model.TotalOrganizationWithValue = model1.TotalOrganizationWithValue;
            model.TotalOrganizationWithoutValue = model1.TotalOrganizationWithoutValue;
            model.CountryList = model2.CountryList;
            model.SectorList = model2.SectorList;
            model.TopTenValueAndCountList= model2.TopTenValueAndCountList;
            model.CountryAndCountList = model3.CountryAndCountList;
            return View(model);
        }
        //public IActionResult LoadDataCountry()
        //{
        //    var model = _CountrySevices.LoadDataTotal();
        //    var models = new HomeViewModel();

        //    return View("Index", model);
        //}
        //public IActionResult Index(int FactionsId, int RegionId, int CityId, int? Page)
        //{
        //    int AllListCount;
        //    var model = new EmergencyPlatoonsModel();
        //    model.FactionsId = FactionsId;
        //    model.RegionId = RegionId;
        //    model.CityId = CityId;
        //    var NewPage = Page.HasValue ? (Page - 1) * 6 : 0;
        //    var list = _EmergencyPlatoonsSevices.LoadDataWeb(out AllListCount, FactionsId, RegionId, CityId, NewPage.Value, 6);
        //    model.LoadDataWeb = new StaticPagedList<EmergencyPlatoonsViewModel>(list, Page ?? 0 + 1, 6, AllListCount);
        //    _initalizeModelLookups.InitModel(model, _localeStringResourceSevices.GetResource("All"));
        //    return View(model);
        //}
        //public IList<EmergencyPlatoons> LoadItemsDataWeb(int FactionsId, int RegionId, int CityId, out int AllListCount, int jtStartIndex = 0,
        //    int jtPageSize = 10, string order = null, string orderDir = null)
        //{
        //    AllListCount = 0;
        //    var dataQuery = _context.EmergencyPlatoons
        //        .Where(x => x.IsPublish && !x.IsDeleted);


        //    if (CityId > 0)
        //    {
        //        dataQuery = dataQuery.Where(s => s.Hospital.CityId == CityId);
        //    }
        //    if (FactionsId > 0)
        //    {
        //        dataQuery = dataQuery.Where(s => s.FactionsId == FactionsId);
        //    }
        //    if (RegionId > 0)
        //    {
        //        dataQuery = dataQuery.Where(s => s.Hospital.RegionId == RegionId);
        //    }

        //    var result = dataQuery.Include(s => s.Factions).Include(x => x.Hospital)
        //       .Distinct().AsQueryable();

        //    if (!string.IsNullOrEmpty(order))
        //    {
        //        switch (order)
        //        {
        //            case "0":
        //                result = orderDir == "asc" ? result.OrderBy(x => x.Factions.Name) : result.OrderByDescending(x => x.Factions.Name);
        //                break;
        //            case "1":
        //                result = orderDir == "asc" ? result.OrderBy(x => x.Hospital.City.Name) : result.OrderByDescending(x => x.Hospital.City.Name);
        //                break;
        //            case "2":
        //                result = orderDir == "asc" ? result.OrderBy(x => x.Hospital.Region.Name) : result.OrderByDescending(x => x.Hospital.Region.Name);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        result = result.OrderByDescending(x => x.Id);
        //    }
        //    if (result.Count() > 0)
        //        AllListCount = dataQuery.Count();

        //    return result.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //}
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
