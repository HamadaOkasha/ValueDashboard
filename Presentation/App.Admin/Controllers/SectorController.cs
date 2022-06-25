using App.Admin.Controllers;
using App.Application.FrameWork;
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
    //
    public class SectorController : BaseController
    {
        private readonly ISectorSevices _SectorSevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public SectorController(ISectorSevices SectorSevices,

            IInitalizeModelLookups initalizeModelLookups, ILocaleStringResourceSevices localeStringResourceSevices)
        {
            _SectorSevices = SectorSevices;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
        }
        public IActionResult Index()
        {
            var model = new SectorModel();

            return View(model);
        }
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            var model = await _SectorSevices.GetById(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(SectorModel model)
        {
            await _SectorSevices.Save(model);
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Sector");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _SectorSevices.Delete(id);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Status(int id, bool status)
        {
            var result = await _SectorSevices.ChangeStatus(id, status);
            return Json(result);
        }

        public async Task<JsonResult> LoadData(string Search, int StatusId)
        {
            // Initialization.
            // string Search = HttpContext.Request.Form["search[value]"][0];
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);
            // Total record count.
            // int totalRecords;
            // Loading.
            var data = await _SectorSevices.LoadData(Search, StatusId,  startRec,
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