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
    public class AlrowadVersionSevices : IAlrowadVersionSevices
    {
        public readonly IAlrowadVersionRepository _AlrowadVersionRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public AlrowadVersionSevices(IAlrowadVersionRepository AlrowadVersionRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _AlrowadVersionRepository = AlrowadVersionRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<AlrowadVersionModel>  GetById(int id)
        {
            var obj = new AlrowadVersion();
            if (id > 0)
                obj = await _AlrowadVersionRepository.GetItemById(id);

            var model = _mapper.Map<AlrowadVersionModel>(obj);
            Action<AlrowadVersionLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(AlrowadVersionModel model)
        {
            var obj = new AlrowadVersion();
            if (model.Id > 0)
                obj = await _AlrowadVersionRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _AlrowadVersionRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _AlrowadVersionRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _AlrowadVersionRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<AlrowadVersionViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _AlrowadVersionRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<AlrowadVersionViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<AlrowadVersionViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(AlrowadVersion AlrowadVersion, AlrowadVersionModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(AlrowadVersion,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }
    }
}
