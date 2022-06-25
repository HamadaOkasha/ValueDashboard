using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Application.Filters;

namespace App.Admin.Controllers
{
    [AuthorizeAdmin]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
    }
}
