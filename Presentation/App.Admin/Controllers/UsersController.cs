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
    public class UsersController : BaseController
    {
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly IUsersService _usersService;
        private readonly IInitalizeModelLookups _initalizeModelLookups;

        public UsersController(IUsersService usersService,
            ILocaleStringResourceSevices localeStringResourceSevices,
            IInitalizeModelLookups initalizeModelLookups)
        {
            _usersService = usersService;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
        }
        public async Task<IActionResult> Index()
        {
            var model = new UserModel();
            await _initalizeModelLookups.InitModel(model, _localeStringResourceSevices.GetResource("All"));

            return View(model);
        }
        public async Task<IActionResult> AddEdit(string id = null)
        {
            var model = new UserModel();
            if (!string.IsNullOrEmpty(id))
            {
                model = await _usersService.GetUserById(id);
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
            }
            await _initalizeModelLookups.InitModel(model, _localeStringResourceSevices.GetResource("Select"));

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(UserModel model)
        {
            var result = await _usersService.CreateUpdateAsync(model);
            if (result != "Succeeded")
            {
                TempData["errorMsg"] = result;
                await _initalizeModelLookups.InitModel(model, _localeStringResourceSevices.GetResource("Select"));
                return View(model);
            }
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Users");
        }
        public IActionResult ChangePassword(string id = null)
        {
            var model = new ChangePasswordModel();
            model.Id = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var result = await _usersService.RestPasswordAsync(model.Id, model.Password);
            if (result != "Succeeded")
            {
                TempData["errorMsg"] = result;
                return View(model);
            }
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Users");
        }
        [HttpPost]
        public async Task<IActionResult> Status(string id, bool status)
        {
           await _usersService.ChangeStatus(id, status);
            return Json(true);
        }

        public async Task<JsonResult> LoadData(string email,  string fullName,  string phoneNumber, string roleName, int statusId)
        {
            // Initialization.
            string draw = HttpContext.Request.Form["draw"][0];
            string order = HttpContext.Request.Form["order[0][column]"][0];
            string orderDir = HttpContext.Request.Form["order[0][dir]"][0];
            int startRec = Convert.ToInt32(HttpContext.Request.Form["start"][0]);
            int pageSize = Convert.ToInt32(HttpContext.Request.Form["length"][0]);
            // Loading.

           
            var data = await _usersService.LoadData(email, fullName, phoneNumber, roleName, statusId, startRec,
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