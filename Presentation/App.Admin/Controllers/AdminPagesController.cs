using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using App.Application.Model;
using System.Threading.Tasks;

namespace App.Admin.Controllers
{
    [Authorize]
    public class AdminPagesController : Controller
    {
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly IAdminPagesSevices _adminPagesSevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        public AdminPagesController(IAdminPagesSevices adminPagesSevices,
             ILocaleStringResourceSevices localeStringResourceSevices,
            IInitalizeModelLookups initalizeModelLookups)
        {
            _adminPagesSevices = adminPagesSevices;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            var model = new AdminPagesModel();
            model = await _adminPagesSevices.GetById(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            await _initalizeModelLookups.InitModel(model, _localeStringResourceSevices.GetResource("NoParentPage"));

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(AdminPagesModel model)
        {
            await  _adminPagesSevices.Save(model);
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "AdminPages");
        }
        public async Task<JsonResult>  LoadData()
        {   
            // Initialization.
            string Search = HttpContext.Request.Form["search[value]"][0];
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);
         
            // Loading.
            var data = await _adminPagesSevices.LoadData(Search, startRec,
                pageSize, order, orderDir);
            
            return Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = data.Item2,
                recordsFiltered = data.Item2,
                data = data.Item1
            });
        }
    }
}