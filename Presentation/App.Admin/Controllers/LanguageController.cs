using App.Application.Filters;
using App.Application.Interfaces;
using App.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace App.Admin.Controllers
{
    [AuthorizeAdmin]
    public class LanguageController : BaseController
    {
        private readonly ILanguageSevices _languageSevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public LanguageController(ILanguageSevices languageSevices,
            ILocaleStringResourceSevices localeStringResourceSevices,
            IInitalizeModelLookups initalizeModelLookups)
        {
            _languageSevices = languageSevices;
            _initalizeModelLookups = initalizeModelLookups;
            _localeStringResourceSevices = localeStringResourceSevices;
        }
        public async Task<IActionResult> Index()
        {
              return View();
        }
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            var model = new LanguageModel();
            if (id > 0)
            {
                model = await _languageSevices.GetById(id);
                model.ResourceModel.LanguageId = id;
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(LanguageModel model)
        {
            await _languageSevices.Save(model);
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Language");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await  _languageSevices.Delete(id);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Status(int id, bool status)
        {
            var result = await  _languageSevices.ChangeStatus(id, status);
            return Json(result);
        }
       
        public async Task<JsonResult> LoadData(string Search,int StatusId)
        {   
            // Initialization.
           // string Search = HttpContext.Request.Form["search[value]"][0];
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);
            // Loading.
            var data = await _languageSevices.LoadData(Search, StatusId, startRec,
                pageSize, order, orderDir);
            
            return Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = data.Item2,
                recordsFiltered = data.Item2,
                data = data.Item1
            });
        }

        #region LocaleStringResource

        public async Task<IActionResult> AddEditLocaleStringResource(int id = 0)
        {
            var model = new LocaleStringResourceModel();
            if (id > 0)
            {
                model = await _localeStringResourceSevices.GetById(id);
            }
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEditLocaleStringResource(LocaleStringResourceModel model)
        {
            var resault = await _localeStringResourceSevices.Save(model);
            if (!resault)
            {
                return Json(_localeStringResourceSevices.GetResource("admin.ResourceNameAlreadyRegistered"));
            }
            return Json("");
        }
        public async Task<JsonResult> LoadDataLocaleStringResource(string ResourceName, string ResourceValue, int languageId)
        {
            // Initialization.
            // string Search = HttpContext.Request.Form["search[value]"][0];
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);

            // Loading.
            var data = await _localeStringResourceSevices.LoadData(ResourceName, ResourceValue, languageId: languageId, startRec,
                pageSize, order, orderDir);

            return Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = data.Item2,
                recordsFiltered = data.Item2,
                data = data.Item1
            });
        }

        #endregion
    }
}