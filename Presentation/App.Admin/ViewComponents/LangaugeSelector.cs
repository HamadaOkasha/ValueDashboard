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
    public class LangaugeSelectorViewComponent : ViewComponent
    {
        private readonly ILanguageSevices _languageSevices;
        public LangaugeSelectorViewComponent(ILanguageSevices languageSevices)
        {
            _languageSevices = languageSevices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _languageSevices.GetListLanguage();

            return View(model);
        }
    }
}
