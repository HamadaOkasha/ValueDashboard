using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using App.Application.Filters;
using System.Security.Claims;
using System.Net.Http;
using App.Application.Framwork;
using Microsoft.Extensions.Configuration;
using App.Application.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using App.Domain.Model;
using App.Application.Models;
using Microsoft.AspNetCore.Http;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AuthController : Controller 
    {
        private readonly IUsersService _usersService;

        public IConfiguration Configuration { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInitalizeModelLookups _initalizeModelLookups;
        private readonly INotificationService _notificationService;
       public AuthController(INotificationService notificationService , IInitalizeModelLookups initalizeModelLookups, IConfiguration configuration, UserManager<ApplicationUser> userManager,
            ILocaleStringResourceSevices localeStringResourceSevices, 
            IUsersService usersService)
        {
            Configuration = configuration;
            _usersService = usersService;
            _userManager = userManager;
            _localeStringResourceSevices = localeStringResourceSevices;
            _initalizeModelLookups = initalizeModelLookups;
          
            _notificationService = notificationService;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var result = await _usersService.SignInAsync(model.UserName, model.Password, model.RememberMe);
            if (result == "Succeeded")
            {
               
                var user = await _userManager.FindByNameAsync(model.UserName);
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: Configuration["JWT:ValidIssuer"],
                    audience: Configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddYears(10),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    token_type = "Bearer",
                    errors = "",
                    userId = user.Id,
                    roles = userRoles,
                });

            }
            else if (result == "EmailNotConfirmed")
            {
                return Ok(new
                {
                    errors = _localeStringResourceSevices.GetResource("EmailNotConfirmation"),
                    token = "",
                    expiration = DateTime.Now,
                });
            }
            else if (result == "IsLockedOut")
            {
                return Ok(new
                {
                    errors = _localeStringResourceSevices.GetResource("IsLockedOut"),
                    token = "",
                    expiration = DateTime.Now,
                });
            }
            else
            {
                return Ok(new
                {
                    errors = _localeStringResourceSevices.GetResource("LoginFaild"),
                    token = "",
                    expiration = DateTime.Now,
                });
            }

            //return Unauthorized();
        }
        [HttpPost]
        [Route("Registered")]
        public async Task<IActionResult> Registered([FromBody] UserModelMobile model)
        {
            model.RoleName = "Customers";
            var result = await _usersService.CreateUpdateMobileAsync(model);
            if (result != "Succeeded")
            {
                return Ok(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result,
                });
            } 
            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = _localeStringResourceSevices.GetResource("SuccessfullyMsg").ToString(),
            });
        }
        [HttpPost]
        public async Task<ActionResult> GetToken([FromForm] UserLoginModel model)
        {
            if (model != null)
            {
                var result = await _usersService.SignInAsync(model.UserName, model.Password, model.RememberMe);
                if (result == "Succeeded")
                {
                    TokenModel token = new TokenModel
                    {
                        token_type = "Bearer",
                        access_token = model.UserName,
                        id_token = model.UserName,
                        expires_in = int.MaxValue,
                        refresh_token = model.UserName
                    };
                    return Ok(token);
                }
                else
                {
                    ModelState.AddModelError("error", "user name or password not correct");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("error", "user name or password not correct");
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        [Route("UpdateCustomerToken")]
        public async Task<ActionResult> UpdateCustomerToken([FromBody] CustomerTokenModel model)
        {
           await _usersService.UpdateCustomerToken(model.Token);
            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = _httpContextAccessor.HttpContext.User.GetId();
            var result = await _usersService.RestPasswordAsync(userId, model.Password);
            if (result != "Succeeded")
            {
                return Ok(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result,
                });
            }
            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = _localeStringResourceSevices.GetResource("SuccessfullyMsg").ToString(),
            });
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _usersService.ForgotPasswordAsync(model.Email);
           
              if (result == "NotFound")
            {
                return Ok(new ResponseModel
                {
                    IsSuccess = false,
                    Message = _localeStringResourceSevices.GetResource("EmailNotRegistered").ToString(),
                });
               
            }
            else if (result == "ForgotPasswordConfirmation")
            {
                return Ok(new ResponseModel
                {
                    IsSuccess = false,
                    Message = _localeStringResourceSevices.GetResource("ForgotPasswordConfirmation"),
                });

            }
            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = _localeStringResourceSevices.GetResource("SuccessfullyMsg").ToString(),
            });
        }
        [HttpPost]
        [Route("UpdateCustomer")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserModelMobile model)
        {
      
            var usermodel = new UserModel();
            usermodel.Email = model.Email;
            usermodel.Adress = model.Adress;
            usermodel.FullName = model.FullName;
            usermodel.PhoneNumber = model.PhoneNumber;
            usermodel.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _usersService.UpdateAsyncMobile(usermodel);
            if (result != "Succeeded")
            {
                return Ok(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result,
                });
            }
            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = _localeStringResourceSevices.GetResource("SuccessfullyMsg").ToString(),
            });
        }

        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new UserModel();
            model = await _usersService.GetUserById(userId);
            await _initalizeModelLookups.InitModel(model);
            return Ok(model);

        }


       

        [HttpGet]
        [Route("LoadNotifcation")]
        [Authorize]
        public async Task<IActionResult> LoadNotifcation(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var List = await   _notificationService.LoadNotifcationapi(jtStartIndex: (pageIndex * pageSize),jtPageSize: pageSize , CurrentCustomerId: User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new
            {
                RecordsTotal = List.Item2,
                Data = List.Item1
            });
        }
    }
}