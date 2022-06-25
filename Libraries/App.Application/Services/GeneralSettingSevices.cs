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
    public class GeneralSettingSevices : IGeneralSettingSevices
    {
        public readonly IGeneralSettingRepository _GeneralSettingRepository;
        private readonly IMapper _mapper;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        public GeneralSettingSevices(IGeneralSettingRepository GeneralSettingRepository,
            ILocalizedModelFactory localizedModelFactory, IMapper mapper)
        {
            _GeneralSettingRepository = GeneralSettingRepository;
            _localizedModelFactory = localizedModelFactory;
            _mapper = mapper;
        }
        public async Task<GeneralSettingModel> GetItem()
        {
            var obj = await _GeneralSettingRepository.GetCurrentItem();

            var model = _mapper.Map<GeneralSettingModel>(obj);

         
            return model;
        }
        public async Task Save(GeneralSettingModel model)
        {
            var obj = new GeneralSetting();
            if (model.Id > 0)
                obj = await _GeneralSettingRepository.GetCurrentItem();
           
            _mapper.Map(model, obj);

            await _GeneralSettingRepository.SaveItem(obj);
        }
    }
}
