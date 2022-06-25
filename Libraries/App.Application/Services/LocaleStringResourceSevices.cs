using App.Application.Caching;
using App.Application.FrameWork;
using App.Application.Framwork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class LocaleStringResourceSevices : ILocaleStringResourceSevices
    {
        private readonly ICacheManager _cacheManager;
        public readonly ILocaleStringResourceRepository _localeStringResourceRepository;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;
        private readonly IConfiguration _Configuration;
        public LocaleStringResourceSevices(ILocaleStringResourceRepository localeStringResourceRepository,
              ICacheManager cacheManager,
             IMapper mapper, IAttachmentService attachmentService, IConfiguration configuration)
        {
            _localeStringResourceRepository = localeStringResourceRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _attachmentService = attachmentService;
            _Configuration = configuration;
        }
        public async Task<LocaleStringResourceModel> GetById(int id)
        {
            var obj = await _localeStringResourceRepository.GetItemById(id);

            var model = _mapper.Map<LocaleStringResourceModel>(obj);

            return model;
        }
        //admin defualt
        //public string GetResource(string resourceKey)
        //{
        //    var CacheKey = string.Format(ModelCacheEventConsumer.RESOURCE_KEY, CommonHelper.WorkingLanguage.Id);

        //    var list = _cacheManager.Get(CacheKey, () =>
        //    {
        //        var list = _localeStringResourceRepository.GetAllResource(CommonHelper.WorkingLanguage.Id).Result;

        //        return list;
        //    });
        //    var item = list.Where(s => s.ResourceName.Trim().ToLower() == resourceKey.Trim().ToLower()).FirstOrDefault();

        //    if (item != null)
        //    {
        //        return item.ResourceValue;
        //    }
        //    else
        //        return resourceKey;

        //}

        public string GetResource(string resourceKey)
        {
            var list = _localeStringResourceRepository.GetAllResource(CommonHelper.WorkingLanguage.Id).Result;
            var item = list.Where(s => s.ResourceName.Trim().ToLower() == resourceKey.Trim().ToLower()).FirstOrDefault();

            if (item != null)
            {
                return item.ResourceValue;
            }
            else
                return resourceKey;

        }

        public async Task<bool> Delete(int id)
        {
            return await _localeStringResourceRepository.Delete(id);
        }
        public async Task<IList<LocalizedString>> GetAllResource()
        {
            //var CacheKey = string.Format(ModelCacheEventConsumer.RESOURCE_KEY, CommonHelper.WorkingLanguage.ResourceModel.LanguageId);
            int langId = CommonHelper.WorkingLanguage.Id;
            var list = await _localeStringResourceRepository.GetAllResource(langId);

            var finalList = (from c in list
                             select new LocalizedString(c.ResourceName.Replace(".", ""), c.ResourceValue)).ToList();

            return finalList;
        }
        public async Task<bool> Save(LocaleStringResourceModel model)
        {
            model.ResourceName = model.ResourceName.Trim().ToLower();

            var checkName = await _localeStringResourceRepository.CheckResourceName(model.ResourceName, model.Id, model.LanguageId);
            if (checkName)
                return false;

            var obj = new LocaleStringResource();
            if (model.Id > 0)
                obj = await _localeStringResourceRepository.GetItemById(model.Id);

            _mapper.Map(model, obj);

            await _localeStringResourceRepository.SaveItem(obj);

            var CacheKey = string.Format(ModelCacheEventConsumer.RESOURCE_KEY, model.LanguageId);
            _cacheManager.Remove(CacheKey);

            return true;
        }


        public async Task<Tuple<IList<LocaleStringResourceViewModel>, int>> LoadData(string ResourceName = null, string ResourceValue = null,
             int languageId = 0, int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _localeStringResourceRepository.LoadItemsData(ResourceName, ResourceValue, languageId,
                jtStartIndex, jtPageSize, order, orderDir);


            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<LocaleStringResourceViewModel>(obj);

                return model;
            });

            return new Tuple<IList<LocaleStringResourceViewModel>, int>(list.ToList(), data.Item2);
        }


        public async Task<IList<LocalizedString>> GetAllResourceMobile(int languageId)
        {
            
          
            var list = await _localeStringResourceRepository.GetAllResourceMobile(languageId);
            var finalList = (from c in list
                             select new LocalizedString(c.ResourceName.Replace("mobile.", ""), c.ResourceValue)).ToList();

            return finalList;
        }


    }
}
