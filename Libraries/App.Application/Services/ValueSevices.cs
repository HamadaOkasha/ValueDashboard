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
    public class ValueSevices : IValueSevices
    {
        public readonly IValueRepository _ValueRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public ValueSevices(IValueRepository ValueRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _ValueRepository = ValueRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<ValueModel>  GetById(int id)
        {
            var obj = new Value();
            if (id > 0)
                obj = await _ValueRepository.GetItemById(id);

            var model = _mapper.Map<ValueModel>(obj);
            Action<ValueLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(ValueModel model)
        {
            var obj = new Value();
            if (model.Id > 0)
                obj = await _ValueRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _ValueRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _ValueRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _ValueRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<ValueViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _ValueRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<ValueViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<ValueViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(Value Value, ValueModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(Value,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }
    }
}
