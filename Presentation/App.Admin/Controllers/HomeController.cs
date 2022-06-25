using App.Application.Caching;
using App.Application.Framwork;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.Admin.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ICacheManager _cacheManager;
        private readonly IConfiguration _Configuration;
        private readonly ILanguageSevices _languageService;
        private readonly INotificationService _notificationService;
 
        private readonly IUsersService _usersService;

        public HomeController(ICacheManager cacheManager, INotificationService notificationService,
            ILanguageSevices languageService, IUsersService usersService,
            IConfiguration configuration)
        {
            _languageService = languageService;
            _cacheManager = cacheManager;

            _usersService = usersService;
            _Configuration = configuration;
            _notificationService = notificationService;
        }
        public IActionResult Index()
        {
            return View();
        }
      
        public virtual async Task<IActionResult> SetLanguage(int langid, string returnUrl = "")
        {
            var language = await _languageService.GetById(langid);

            CommonHelper.WorkingLanguage = language;

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("Homepage");

            return Redirect(returnUrl);
        }
        //[Authorize]
        //public IActionResult ClearCash()
        //{
        //    try
        //    {
        //        _cacheManager.Clear();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return RedirectToAction("Index");
        //}
        [Authorize]
        public IActionResult ClearCash()
        {
            _cacheManager.Clear();

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string wsUrl = _Configuration["ApiUrl"];
            string Url = wsUrl + "api/Common/ClearAllCash";
            WebRequest webRequest = WebRequest.Create(Url);
            webRequest.GetResponse();
            return RedirectToAction("Index");
        }
        public async Task< IActionResult> ReloadNotifications()
        {
            int totalRecords = 0;
            // Loading.
            var model = await _notificationService.LoadNotifcation(
                  totalRecords, 0, 8, null, false);

            return PartialView("_NotificationsList", model.Item1);
        }

        public async Task<IActionResult> LoadChartCustomerTotal()
        {
            var data = await _usersService.LoadChartCustomerTotal("Customers");

            var listStatus = data.GroupBy(g => g.CustomerId).Select(x => new
            {
                name = "Customers",
                type = "bar",
                stack = "bar",
                data = data.Where(s => s.CustomerId == x.Key).Select(g => g.Total).ToList()
            }).Distinct().ToList();
            return Json(new
            {
                dates = data.Select(s => s.Date).Distinct().ToList(),
                //status = data.Select(s => s.CustomerId).Distinct().ToList(),
                status = "Customers",

                listStatus = listStatus
            });
        }


        //public async Task<IActionResult> LoadChartCustomerTotal(string roleName)
        //{
        //    var data = await _usersService.LoadChartCustomerTotal(roleName);

        //    var listCustomer = data.Select(x => new
        //    {
        //        name = x.re,
        //        value = x.Total,
        //    }).Distinct().ToList();

        //    return Json(new
        //    {
        //        Customer = data.Select(s => s.CustomerId).Distinct().ToList(),
        //        listCustomer = listCustomer,
        //    });
        //}
    }
}
