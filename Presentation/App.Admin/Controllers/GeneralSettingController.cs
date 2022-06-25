using App.Application.Interfaces;
using App.Application.Model;
using App.Application.Services;
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
    public class GeneralSettingController : BaseController
    {
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly IGeneralSettingSevices _GeneralSettingSevices;
        public GeneralSettingController(IGeneralSettingSevices GeneralSettingSevices,
                 ILocaleStringResourceSevices localeStringResourceSevices)
        {
            _GeneralSettingSevices = GeneralSettingSevices;
            _localeStringResourceSevices = localeStringResourceSevices;
        }

       
        public async Task<IActionResult> Index()
        {

            var model = await _GeneralSettingSevices.GetItem();

            if (model == null)
            {
                model = new GeneralSettingModel();
                model.Id = 0;
            }
            return View(model);
        }
        [HttpPost]
        public async Task< IActionResult> Index(GeneralSettingModel model)
        {
            await _GeneralSettingSevices.Save(model);
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "GeneralSetting");
        }
    }
}