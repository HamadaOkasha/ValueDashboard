using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using App.Application.Model;
using App.Domain.Model;
using Microsoft.AspNetCore.Http;
using App.Application.Framwork;

namespace App.Application.Services
{
    public class ResearchValueSevices : IResearchValueSevices
    {
        public readonly IResearchValueRepository _ResearchValueRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public ResearchValueSevices(IResearchValueRepository ResearchValueRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _ResearchValueRepository = ResearchValueRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<ResearchValueModel>  GetById(int id)
        {
            var obj = new ResearchValue();
            if (id > 0)
                obj = await _ResearchValueRepository.GetItemById(id);

            var model = _mapper.Map<ResearchValueModel>(obj);
            Action<ResearchValueLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(ResearchValueModel model)
        {
            var obj = new ResearchValue();
            if (model.Id > 0)
                obj = await _ResearchValueRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _ResearchValueRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _ResearchValueRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _ResearchValueRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<ResearchValueViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _ResearchValueRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<ResearchValueViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<ResearchValueViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(ResearchValue ResearchValue, ResearchValueModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(ResearchValue,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }
    }
}
