using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using App.Application.Caching;
using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

using App.Application.Framwork;

namespace App.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMessagesTemplateRepository _messagesTemplateRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly ILanguageSevices _languageSevices;
        private readonly INotificationService _notificationService;
        public UsersService(INotificationService notificationService ,IUserRepository userRepository, ILanguageSevices languageSevices,
            ICacheManager cacheManager,
            SignInManager<ApplicationUser> signInManager,
             ILocaleStringResourceSevices localeStringResourceSevices,
            IEmailSender emailSender,
             UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMessagesTemplateRepository messagesTemplateRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _cacheManager = cacheManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _messagesTemplateRepository = messagesTemplateRepository;
            _emailSender = emailSender;
            _localeStringResourceSevices = localeStringResourceSevices;
            _languageSevices = languageSevices;
            _notificationService = notificationService;
        }
        public async Task<string> SignInAsync(string userName, string password, bool isPersistent, bool loginFromPortal = false, string returnUrl = null)
        {
            var userInfo = await _userManager.FindByNameAsync(userName);
            if (userInfo == null)
                userInfo = await _userManager.FindByEmailAsync(userName);

            if (userInfo == null)
                return "NotFound";

            var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, password, isPersistent, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return "Succeeded";
            }
            else if (result.RequiresTwoFactor)
                return "RequiresTwoFactor";
            else if (result.IsLockedOut)
                return "IsLockedOut";
            else
                return "NotFound";
        }
        public async Task ChangeStatus(string id, bool status)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            var Result = await _userManager.SetLockoutEnabledAsync(identityUser, status);
        }
        public async Task UpdateCustomerToken(string token)
        {

            string userId = _httpContextAccessor.HttpContext.User.GetId();
            await _userRepository.UpdateCustomerToken(userId, token);
        }
        public async Task<UserModel> GetUserById(string userId)
        {
            var obj = await _userManager.FindByIdAsync(userId);
            var objFullInfo = await _userRepository.GetUserById(userId);

            var model = _mapper.Map<UserModel>(obj);
            model.FullName = objFullInfo.FullName;
            //model.Adress = objFullInfo.Adress;

            var listRoles = await _userManager.GetRolesAsync(obj);

            if (listRoles.Any())
                model.RoleName = listRoles.FirstOrDefault();

            return model;
        }

        public async Task<UserViewModel> GetUserDataByEmail(string email)
        {
            var objFullInfo = await _userRepository.GetUserByEmail(email);

            var model = _mapper.Map<UserViewModel>(objFullInfo);

            if(objFullInfo.AspNetUserRoles.Any())
            {
                model.RoleName = objFullInfo.AspNetUserRoles.FirstOrDefault().Role.Name;
                model.IsMembers = objFullInfo.AspNetUserRoles.FirstOrDefault().Role.IsMembers;
            //    var listPages = GetUserAccessPages();
              //  model.IsPaymentAccess = listPages.Where(s => s.PageUrl == "Payment").Any();
            }
            return model;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<string> CreateUpdateAsync(UserModel model)
        {
            bool IsPhoneAlreadyRegistered = _userManager.Users.Any(item => item.PhoneNumber == model.PhoneNumber
            && item.Id != model.Id);
            if (IsPhoneAlreadyRegistered)
            {
                return "Phone number '" + model.PhoneNumber + "' is already taken.";
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                var obj = new ApplicationUser();
                _mapper.Map(model, obj);

                obj.UserName = model.Email;
                obj.Adress = model.Adress;
                obj.UnitNumber = model.UnitNumber;
                
                obj.NormalizedUserName = model.Email.ToUpper();
                obj.NormalizedEmail = model.Email.ToUpper();
                obj.EmailConfirmed = true;
                obj.PhoneNumberConfirmed = true;
                obj.Id = Guid.NewGuid().ToString();
                var objuser = new AspNetUsers();

                var result = await _userManager.CreateAsync(obj, model.Password);
                var userinfo = await _userRepository.GetUserByEmail(model.Email);
                if (result.Succeeded)
                {
                    _notificationService.SendNotifications(new NotificationModel()
                    {
                        FromCustomerId = userinfo.Id,
                        ToCustomerId = null,
                        MessageBody = string.Format(_localeStringResourceSevices.GetResource("newuserisregistered"),model.FullName)

                    }, userinfo.Id);
                }

                if (!result.Succeeded)
                {
                    return string.Join("<br />", result.Errors.Select(s => s.Description));
                }
                else
                {
                    await _userManager.AddToRoleAsync(obj, model.RoleName);

                    await _userManager.SetLockoutEnabledAsync(obj, obj.LockoutEnabled);

                    return "Succeeded";
                }
            }
            else
            {
                var obj = await _userManager.FindByIdAsync(model.Id);

                _mapper.Map(model, obj);

                obj.UserName = model.Email;
                obj.NormalizedUserName = model.Email.ToUpper();
                obj.NormalizedEmail = model.Email.ToUpper();
                obj.Adress = model.Adress;
                obj.UnitNumber = model.UnitNumber;

                var result = await _userManager.UpdateAsync(obj);
             
                if (!result.Succeeded)
                {
                    return string.Join("<br />", result.Errors.Select(s => s.Description));
                }
                else
                {
                    if (!await _userManager.IsInRoleAsync(obj, model.RoleName))
                    {
                        await _userManager.AddToRoleAsync(obj, model.RoleName);
                    }
                    return "Succeeded";
                }
            }
        }

        public async Task<string> CreateUpdateMobileAsync(UserModelMobile model)
        {
            bool IsPhoneAlreadyRegistered = _userManager.Users.Any(item => item.PhoneNumber == model.PhoneNumber
            && item.Id != model.Id);
            if (IsPhoneAlreadyRegistered)
            {
                return "Phone number '" + model.PhoneNumber + "' is already taken.";
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                var obj = new ApplicationUser();
                _mapper.Map(model, obj);

                obj.UserName = model.Email;
                obj.Adress = model.Adress;
                obj.UnitNumber = model.UnitNumber;

                obj.NormalizedUserName = model.Email.ToUpper();
                obj.NormalizedEmail = model.Email.ToUpper();
                obj.EmailConfirmed = true;
                obj.PhoneNumberConfirmed = true;
                obj.Id = Guid.NewGuid().ToString();
                var objuser = new AspNetUsers();

                var result = await _userManager.CreateAsync(obj, model.Password);
                var userinfo = await _userRepository.GetUserByEmail(model.Email);
                if (result.Succeeded)
                {
                    _notificationService.SendNotifications(new NotificationModel()
                    {
                        FromCustomerId = userinfo.Id,
                        ToCustomerId = null,
                        MessageBody = string.Format(_localeStringResourceSevices.GetResource("newuserisregistered"), model.FullName)

                    }, userinfo.Id);
                }

                if (!result.Succeeded)
                {
                    return string.Join("<br />", result.Errors.Select(s => s.Description));
                }
                else
                {
                    await _userManager.AddToRoleAsync(obj, model.RoleName);

                    await _userManager.SetLockoutEnabledAsync(obj, obj.LockoutEnabled);

                    return "Succeeded";
                }
            }
            else
            {
                var obj = await _userManager.FindByIdAsync(model.Id);

                _mapper.Map(model, obj);

                obj.UserName = model.Email;
                obj.NormalizedUserName = model.Email.ToUpper();
                obj.NormalizedEmail = model.Email.ToUpper();
                obj.Adress = model.Adress;
                obj.UnitNumber = model.UnitNumber;

                var result = await _userManager.UpdateAsync(obj);

                if (!result.Succeeded)
                {
                    return string.Join("<br />", result.Errors.Select(s => s.Description));
                }
                else
                {
                    if (!await _userManager.IsInRoleAsync(obj, model.RoleName))
                    {
                        await _userManager.AddToRoleAsync(obj, model.RoleName);
                    }
                    return "Succeeded";
                }
            }
        }

        public async Task<string> UpdateAsyncMobile(UserModel model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                var obj = await _userManager.FindByIdAsync(model.Id);
                obj.FullName = model.FullName;
                obj.PhoneNumber = model.PhoneNumber;
                obj.UserName = model.Email;
                obj.NormalizedUserName = model.Email.ToUpper();
                obj.NormalizedEmail = model.Email.ToUpper();
                obj.Adress = model.Adress;
                obj.UnitNumber = model.UnitNumber;

                var result = await _userManager.UpdateAsync(obj);

                if (!result.Succeeded)
                {
                    return string.Join("<br />", result.Errors.Select(s => s.Description));
                }
                else
                {

                    return "Succeeded";
                }
            }
            else
                return "UserNotFound";
               
       
        }
        public async Task<string> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (changePasswordResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return "Succeeded";
            }
            else
            {
                return changePasswordResult.Errors.FirstOrDefault().Description;
            }
        }
        public async Task<string> RestPasswordAsync(string userId, string newPassword)
        {
            var user = new ApplicationUser();
            if (string.IsNullOrEmpty(userId))
            {
                user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

                userId = user.Id;
            }
            user = await _userManager.FindByIdAsync(userId);

            await _userManager.RemovePasswordAsync(user);
            var changePasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (changePasswordResult.Succeeded)
            {
                return "Succeeded";
            }
            else
            {
                return changePasswordResult.Errors.FirstOrDefault().Description;
            }
        }

        public async Task<IList<AdminPagesViewModel>> GetUserAccessPages()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var roleNames = await _userManager.GetRolesAsync(user);

            var roleName = (roleNames).FirstOrDefault();

            var menuCacheKey = string.Format(ModelCacheEventConsumer.MENU_KEY, roleName, CommonHelper.WorkingLanguage.Id);

            
                var listLookup = await _userRepository.GeAdminPagesList(roleNames.ToArray());
             return await listLookup.SelectAwait(async obj =>
                {
                    var model = new AdminPagesViewModel();
                    model.PageTitle = await _languageSevices.GetLocalized(obj, entity => entity.PageTitle , languageId: CommonHelper.WorkingLanguage.Id);
                    model.Id = obj.Id;
                    model.Icon = obj.Icon;
                    model.PageUrl = obj.PageUrl;
                    model.ParentId = obj.ParentId;
                    model.DisplayOrder = obj.DisplayOrder ?? 0;
                    return model;
                }).ToListAsync();

             
          
        }
        //public async Task<IList<AdminPagesViewModel>> GetUserAccessPages()
        //{
        //    var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        //    var roleNames = await _userManager.GetRolesAsync(user);

        //    var roleName =  (roleNames).FirstOrDefault();

        //    var menuCacheKey = string.Format(ModelCacheEventConsumer.MENU_KEY, roleName);

        //    return _cacheManager.Get(menuCacheKey, () =>
        //    {
        //        var listLookup = _userRepository.GeAdminPagesList(roleNames.ToArray()).Result;
        //        var result = (listLookup.Select(s => new AdminPagesViewModel()
        //        {
        //            Id = s.Id,
        //            PageTitle =  s.PageTitle,
        //            Icon = s.Icon,
        //            PageUrl = s.PageUrl,
        //            ParentId = s.ParentId,
        //        })).ToList();

        //        return result;
        //    });
        //}
        public async Task<bool> CheckUserAccessPage(string page)
        {
            var listPages = await GetUserAccessPages();

            return listPages.Where(s => s.PageUrl == page).Any();
        }
        public async Task<string> ForgotPasswordAsync(string email, bool forgertFromPortal = false)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "NotFound";

            var roleCheck = await _userManager.IsInRoleAsync(user, "PortalUsers");

            if (forgertFromPortal && !roleCheck)
                return "NotFound";
            else if (!forgertFromPortal && roleCheck)
                return "NotFound";

            if (!user.EmailConfirmed)
            {
                return "ForgotPasswordConfirmation";
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = string.Format("{0}{1}{2}{3}",
                _httpContextAccessor.HttpContext.Request.Scheme + "://",
                _httpContextAccessor.HttpContext.Request.Host,
                "/Account/ResetPassword?code=",
                code);

            var body = "";
            var templete = await _messagesTemplateRepository.GetMessagesTemplateById(2);

            body = templete.Description;
            body = body.Replace("{Name}", user.UserName);
            body = body.Replace("{Url}", "<a href='" + HtmlEncoder.Default.Encode(callbackUrl) + "'>" + _localeStringResourceSevices.GetResource("clickHere") + "</a>.");

            await _emailSender.SendEmailAsync(email, _localeStringResourceSevices.GetResource("forgetPassword"), body);

            return "Succeeded";
        }
        public async Task<string> ResetPasswordAsync(string email, string password, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "NotFound";
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return "Succeeded";
            }
            var Errors = new List<string>();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.Description);
            }
            return string.Join("<br />", Errors);
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                //   ChangeStatus(userId, false);
                return true;
            }

            return false;
        }
        public async Task<Tuple<IList<UserViewModel>,int>> LoadData( string email, string fullName, string phoneNumber, string roleName, int StatusId, int jtStartIndex = 0,
              int jtPageSize = 10, string order = null, string orderDir = null)
        {

            var data = await _userRepository.LoadItemsData( email, fullName, phoneNumber,roleName,StatusId,  jtStartIndex,
                jtPageSize, order, orderDir);

            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<UserViewModel>(obj);

                var identityUser = _userManager.FindByIdAsync(obj.Id).Result;
                model.RoleName = _userManager.GetRolesAsync(identityUser).Result.FirstOrDefault();
                return model;
            });

            return new Tuple<IList<UserViewModel>, int>(list.ToList(),data.Item2);
        }
        public async Task<string> GetUser()
        { 
                return  _httpContextAccessor.HttpContext.User.GetId(); 
        }


        public async Task<IList<CustomerChartViewModel>> LoadChartCustomerTotal(string roleName)
        {
            var endDate = DateTime.Now.ReternLocalDate();
            var startDate = endDate.AddDays(-15);
            var list = new List<CustomerChartViewModel>();
            var listAds = await _userRepository.LoadChartCustomer(roleName,startDate, endDate);
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
            {
                if (listAds.Any())
                {
                    foreach (var group in listAds.GroupBy(s => s.AspNetUserRoles.Where(s => s.RoleId == roleName).Any()))
                    {
                        var model = new CustomerChartViewModel();

                        var item = listAds.Where(g => g.CreateDate == date.Date).FirstOrDefault();
                        if (item != null)
                        {
                            model.Date = item.CreateDate.Value.ToString("dd/MM/yyyy");
                            model.Total = item.UnitNumber;
                            model.CustomerId = roleName;
                        }
                        else
                        {
                            model.Date = date.ToString("dd/MM/yyyy");

                        }
                        list.Add(model);
                    }
                }
                else
                {
                    var model = new CustomerChartViewModel();
                    model.Date = date.ToString("dd/MM/yyyy");
                    list.Add(model);
                }
            }

            return list;
        }
    }
}
