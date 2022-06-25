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
    public class NotificationController : BaseController
    {
        private readonly INotificationService _NotificationSevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public NotificationController(INotificationService NotificationSevices,

            IInitalizeModelLookups initalizeModelLookups, ILocaleStringResourceSevices localeStringResourceSevices)
        {
            _NotificationSevices = NotificationSevices;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
        }
        public  IActionResult Index()
        {
            var model = new NotificationModel();

            return View(model);
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
            int totalRecords = 0 ;
            // int totalRecords;
            // Loading.
            var data = await _NotificationSevices.LoadNotifcation( totalRecords, startRec,
                pageSize,null, Search: Search, order: order, orderDir: orderDir);

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