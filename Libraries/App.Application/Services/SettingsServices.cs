using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App.Application.Services
{
    public class SettingsServices : ISettingsServices
    {
        private readonly IMapper _mapper;
        private readonly ISettingsRepository _settingsRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SettingsServices(ISettingsRepository settingsRepository,
                            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _settingsRepository = settingsRepository;
            _mapper = mapper;

        }

        public async Task<MailSettingModel> LoadMailSetting()
        {
            var obj = await _settingsRepository.LoadMailSetting();
            if (obj == null)
                return new MailSettingModel();

            var model = _mapper.Map<MailSettingModel>(obj);

            return model;
        }

        //public SmsconfigModel LoadSmsSetting()
        //{
        //    var obj = _settingsRepository.LoadSmsSetting();
        //    if (obj == null)
        //        return new SmsconfigModel();

        //    var model = _mapper.Map<SmsconfigModel>(obj);

        //    return model;
        //}
        public async Task SaveMailSetting(MailSettingModel model)
        {
            var obj = new MailSetting();
            if (model.Id > 0)
                obj = await _settingsRepository.LoadMailSetting();

            _mapper.Map(model, obj);

           await _settingsRepository.SaveMailSetting(obj);
        }
        //public void SaveSmsSetting(SmsconfigModel model)
        //{
        //    var obj = new Smsconfig();
        //    if (model.Id > 0)
        //        obj = _settingsRepository.LoadSmsSetting();

        //    _mapper.Map(model, obj);

        //    _settingsRepository.SaveSmsSetting(obj);
        //}
        
    }
}
