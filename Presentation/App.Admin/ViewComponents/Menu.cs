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
    public class MenuViewComponent : ViewComponent
    {
        private readonly IUsersService _usersService;
        public MenuViewComponent(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _usersService.GetUserAccessPages();
            return View(model);
        }
    }
}
