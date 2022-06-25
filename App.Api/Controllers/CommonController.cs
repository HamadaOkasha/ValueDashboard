using App.Application.Caching;
using App.Application.Filters;
using App.Application.FrameWork;
using App.Application.Framwork;
using App.Application.Interfaces;
using App.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    
    public class CommonController : Controller
    {
    
        private readonly ILookupSevices _lookupSevices;
        private readonly IGroupService _groupService;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly ICacheManager _cacheManager;
        private readonly ICountrySevices _CategorySevices;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly IUsersService _usersService;
        private readonly ILanguageSevices _languageServices;

        public CommonController(ILanguageSevices languageServices , ILookupSevices lookupSevices, IUsersService usersService,
            ICacheManager cacheManager,
           
            ICountrySevices CategorySevices,
            IGroupService groupService, IInitalizeModelLookups initalizeModelLookups ,
            ILocaleStringResourceSevices localeStringResourceSevices) 
        {
            _groupService = groupService;
            _lookupSevices = lookupSevices;
            _localeStringResourceSevices = localeStringResourceSevices;
            _cacheManager = cacheManager;
         
            _CategorySevices = CategorySevices;
            _usersService = usersService;
            _languageServices = languageServices;


        }
       
        [HttpGet]
        [Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList(string selectTitle = null)
        {
            var Category = await _groupService.GeCategoryList(selectTitle);
            return Ok(Category);
        }
    
        [HttpGet]
        [Route("ClearAllCash")]
        public IActionResult ClearAllCash()
        {
            _cacheManager.Clear();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllResource")]
        public async Task<IActionResult> GetAllResource(int languageId)
        {
            var Status = await _localeStringResourceSevices.GetAllResourceMobile(languageId);
            return Ok(Status);
        }
        [HttpGet]
        [Route("GetLanguageList")]
        public async Task<IActionResult> GetLanguageList(string selectTitle = null)
        {
            var Category = await _languageServices.GeLanguageList(selectTitle);
            return Ok(Category);
        }

    }
}
