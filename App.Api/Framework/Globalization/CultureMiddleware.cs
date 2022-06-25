using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Framwork;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace App.Api.Framework.Globalization
{
    /// <summary>
    /// Represents middleware that set current culture based on request
    /// </summary>
    public class CultureMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">Next</param>
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Set working culture
        /// </summary>
        /// <param name="webHelper">Web helper</param>
        /// <param name="workContext">Work context</param>
        protected void SetWorkingCulture(IHttpContextAccessor httpContextAccessor)
        {
            CultureInfo cultureInfo = null;
            if (CommonHelper.WorkingLanguage != null)
            {
                //set working language culture
                cultureInfo = new CultureInfo(CommonHelper.WorkingLanguage.LanguageCulture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }
            else
            {
                //set working language culture
                cultureInfo = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }
            var requestCulture = new RequestCulture(cultureInfo, cultureInfo);

            IRequestCultureProvider winningProvider = null;

            httpContextAccessor.HttpContext.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, winningProvider));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke middleware actions
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Task</returns>
        public Task Invoke(HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            //set culture
            SetWorkingCulture(httpContextAccessor);

            //call the next middleware in the request pipeline
            return _next(context);
        }
        
        #endregion
    }
}
