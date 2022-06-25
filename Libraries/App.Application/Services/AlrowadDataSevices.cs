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
    public class AlrowadDataSevices : IAlrowadDataSevices
    {
        public readonly IAlrowadDataRepository _AlrowadDataRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public AlrowadDataSevices(IAlrowadDataRepository AlrowadDataRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _AlrowadDataRepository = AlrowadDataRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<AlrowadDataModel>  GetById(int id)
        {
            var obj = new AlrowadData();
            if (id > 0)
                obj = await _AlrowadDataRepository.GetItemById(id);

            var model = _mapper.Map<AlrowadDataModel>(obj);
           // Action<AlrowadDataLocalizedModel, int> localizedModelConfiguration = null;
            //localizedModelConfiguration = (locale, languageId) =>
            //{
            //    locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            //};
          //  model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(AlrowadDataModel model)
        {
            var obj = new AlrowadData();
            if (model.Id > 0)
                obj = await _AlrowadDataRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _AlrowadDataRepository.SaveItem(obj);

            //await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _AlrowadDataRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _AlrowadDataRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<AlrowadDataViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _AlrowadDataRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<AlrowadDataViewModel>(obj);
                if (obj.Country != null)
                {
                    model.CountryId = obj.Country.Name;
                }

                if (obj.Category != null)
                {
                    model.CategoryId = obj.Category.Name;
                }
                if (obj.Organization != null)
                {
                    model.OrganizationId = obj.Organization.Name;
                }
                if (obj.Sector != null)
                {
                    model.SectorId = obj.Sector.Name;
                }
                if (obj.Value != null)
                {
                    model.ValueId = obj.Value.Name;
                }
                if (obj.ResearchValue != null)
                {
                    model.ResearchValueId = obj.ResearchValue.Name;
                }
                if (obj.AlrowadVersion != null)
                {
                    model.AlrowadVersionId = obj.AlrowadVersion.Name;
                }
                //model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<AlrowadDataViewModel>, int>(list, data.Item2);
        }
        //public async Task UpdateLocales(AlrowadData AlrowadData, AlrowadDataModel model)
        //{
        //    //foreach (var localized in model.Locales)
        //    //{
        //    //  await  _languageSevices.SaveLocalizedValue(AlrowadData,
        //    //         x => x.Name,
        //    //         localized.Name,
        //    //         localized.LanguageId);
        //    //}
        //}
    }
}
