using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using App.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Filters
{
    public sealed class AuthorizeAdminAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        /// <param name="ignore">Whether to ignore the execution of filter actions</param>
        public AuthorizeAdminAttribute() : base(typeof(AuthorizeAdminFilter))
        {
        }

        #endregion

        #region Nested filter
        private class AuthorizeAdminFilter : IAsyncAuthorizationFilter
        {
            #region Fields

            private readonly IUsersService _usersService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            #endregion

            #region Ctor

            public AuthorizeAdminFilter(IUsersService usersService,
                IHttpContextAccessor httpContextAccessor)
            {
                _usersService = usersService;
                _httpContextAccessor = httpContextAccessor;
            }
            public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
            {
                try
                {
                    if (filterContext == null)
                        throw new ArgumentNullException(nameof(filterContext));

                    var ctrName = filterContext.RouteData.Values["controller"].ToString();

                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        var check = await _usersService.CheckUserAccessPage(ctrName);
                        if (!check)
                        {
                            filterContext.Result = new ForbidResult();
                        }
                    }
                    else
                        filterContext.Result = new RedirectResult("~/Account/AccessDenied");

                }
                catch (Exception ex)
                {
                    filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                }
            }

            #endregion
        }
        #endregion
    }
}
