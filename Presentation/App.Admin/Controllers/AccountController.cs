using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;
using App.Application.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.Application.FrameWork;


//test
namespace App.Admin.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IEmailSender _emailSender;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public AccountController(IUsersService usersService,
             ILocaleStringResourceSevices localeStringResourceSevices,
            IEmailSender emailSender)
        {
            _usersService = usersService;
            _emailSender = emailSender;
            _localeStringResourceSevices = localeStringResourceSevices;
        }
        public IActionResult Login(string returnUrl)
        {
            UserLoginModel model = new UserLoginModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var result = await _usersService.SignInAsync(model.UserName, model.Password, model.RememberMe);
            if (result == "Succeeded")
                return RedirectToLocal(model.ReturnUrl);
            else if (result == "EmailNotConfirmed")
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("EmailNotConfirmation");
            }
            else if (result == "IsLockedOut")
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("IsLockedOut");
            }
            else
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("LoginFaild");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignOut(string returnUrl)
        {
            await _usersService.SignOutAsync();
            return RedirectToLocal(returnUrl);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {   
            var result = await _usersService.RestPasswordAsync(null, model.Password);
           // var result = await _usersService.ChangePasswordAsync(model.OldPassword, model.Password);
            if (result != "Succeeded")
            {
                TempData["errorMsg"] = result;
                return View(model);
            }
            TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var result = await _usersService.ForgotPasswordAsync(model.Email);
            if (result == "Succeeded")
            {
                TempData["successMsg"] = _localeStringResourceSevices.GetResource("RetrievePasswordMsg");
            }
            else if (result == "NotFound")
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("EmailNotRegistered"); 
                return View(model);
            }
            else if (result == "ForgotPasswordConfirmation")
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("ForgotPasswordConfirmation");
            }
            return RedirectToAction("Index", "Home");

        }
        public IActionResult ResetPassword(string code)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.Code = code;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var result = await _usersService.ResetPasswordAsync(model.Email, model.Password, model.Code);
            if (result == "Succeeded")
            {
                TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyMsg");
                return RedirectToAction("Index", "Home");
            }
            else if (result == "NotFound")
            {
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("EmailNotRegistered"); 
            }
            else
            {
                TempData["errorMsg"] = result;
            }
            return RedirectToAction("ResetPassword", "Account",new { code = model.Code});
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var result = await _usersService.ConfirmEmailAsync(userId, code);
            if (!result)
                TempData["errorMsg"] = _localeStringResourceSevices.GetResource("ErrorConfirm"); 
            else
                TempData["successMsg"] = _localeStringResourceSevices.GetResource("SuccessfullyConfirmMsg"); 

            return RedirectToLocal(returnUrl);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }
    }
}