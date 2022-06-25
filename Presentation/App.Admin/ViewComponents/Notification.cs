using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Admin.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService ;
        public NotificationViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int totalRecords = 0;
            // Loading.
            var model = await _notificationService.LoadNotifcation(totalRecords, 0, 8, null, false);


            return View(model.Item1);
        }
    }
}







