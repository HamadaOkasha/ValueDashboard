using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using App.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Application.Framwork;
using App.Application.Services;
using App.Application.Caching;
using System.Threading.Tasks;

namespace App.Application.Filters
{
    public sealed class HeadersLanguageAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var _languageSevices = EngineContext.Current.Resolve<ILanguageSevices>();
            var _cacheManager = EngineContext.Current.Resolve<ICacheManager>();
            // Grab the current request headers
            var headers = filterContext.HttpContext.Request.Headers;

            // Ensure that all of your properties are present in the current Request
            if (!String.IsNullOrEmpty(headers["Accept-Language"]))
            {
                var LanguageCulture = headers["Accept-Language"];

                var CacheKey = string.Format(ModelCacheEventConsumer.Language_KEY, LanguageCulture);

                var language = await _cacheManager.GetAsync(CacheKey, async () =>
                {
                    await _languageSevices.GetAllLocalizedProperties();

                    var language = await _languageSevices.GetLanguageByLanguageCulture(LanguageCulture);

                    return language;
                });

                CommonHelper.WorkingLanguage = language;
            }

            await next();

            // Additional Auditing-based Logic Here
            base.OnActionExecuting(filterContext);

        }
    }
}
