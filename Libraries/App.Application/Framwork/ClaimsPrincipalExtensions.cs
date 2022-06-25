using App.Application.Interfaces;
using App.Application.Services;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace App.Application.Framwork
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal principal)
        {
            var userId = principal?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
            return userId;
        }
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            var email = principal?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;

            return email;
        }
    }
}
