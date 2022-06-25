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
    public class SectorSevices : ISectorSevices
    {
        public readonly ISectorRepository _SectorRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public SectorSevices(ISectorRepository SectorRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _SectorRepository = SectorRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<SectorModel>  GetById(int id)
        {
            var obj = new Sector();
            if (id > 0)
                obj = await _SectorRepository.GetItemById(id);

            var model = _mapper.Map<SectorModel>(obj);
            Action<SectorLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(SectorModel model)
        {
            var obj = new Sector();
            if (model.Id > 0)
                obj = await _SectorRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _SectorRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _SectorRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _SectorRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<SectorViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _SectorRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<SectorViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<SectorViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(Sector Sector, SectorModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(Sector,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }
    }
}
