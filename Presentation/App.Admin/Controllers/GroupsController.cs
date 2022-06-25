using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using App.Application.Model;

namespace App.Admin.Controllers
{
    [Authorize]
    public class GroupsController : BaseController
    {
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly IGroupService _GroupService;
        private readonly IInitalizeModelLookups _initalizeModelLookups;

        public GroupsController(IGroupService GroupService,
            IInitalizeModelLookups initalizeModelLookups, 
            ILocaleStringResourceSevices localeStringResourceSevices)
        {
            _GroupService = GroupService;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddEdit(string id = null)
        {
            var model = new GroupModel();
            model = await _GroupService.GetById(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            await _initalizeModelLookups.InitModel(model);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(GroupModel model)
        {
            var result = await _GroupService.Save(model);

            if (!result)
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("GroupNameExisting");
                await _initalizeModelLookups.InitModel(model);
                return View(model);
            }
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Groups");
        }
        public async Task<JsonResult> LoadData()
        {   
            // Initialization.
            string Search = HttpContext.Request.Form["search[value]"][0];
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);
            // Loading.

           
            var data = await _GroupService.LoadData(Search,  startRec,
           pageSize, order, orderDir);

            return Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = data.Item2,
                recordsFiltered = data.Item2,
                data = data.Item1
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _GroupService.Delete(id);
            return Json(result);
        }
    }
}