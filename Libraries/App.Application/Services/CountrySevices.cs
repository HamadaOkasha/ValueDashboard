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
    public class CountrySevices : ICountrySevices
    {
        public readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public CountrySevices(ICountryRepository CountryRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _CountryRepository = CountryRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<CountryModel>  GetById(int id)
        {
            var obj = new Country();
            if (id > 0)
                obj = await _CountryRepository.GetItemById(id);

            var model = _mapper.Map<CountryModel>(obj);
            Action<CountryLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(CountryModel model)
        {
            var obj = new Country();
            if (model.Id > 0)
                obj = await _CountryRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _CountryRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _CountryRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _CountryRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<CountryViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _CountryRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<CountryViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<CountryViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(Country Country, CountryModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(Country,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }

        public HomeViewModel LoadDataTotal(int? country)
        {

            var model = new HomeViewModel();

            model.TotalCategory = _CountryRepository.LoadItemsDataCategory(country);
            model.TotalCountry = _CountryRepository.LoadItemsDataCountry(country);
            model.TotalOrganization = _CountryRepository.LoadItemsDataOrganization();
            model.TotalSector = _CountryRepository.LoadItemsDataSector();
            model.TotalOrganizationWithoutValue = _CountryRepository.LoadItemsDataOrganzationWithoutValue();
            model.TotalOrganizationWithValue = _CountryRepository.LoadItemsDataOrganzationWithValue();
            model.TotalValue = _CountryRepository.LoadItemsDataValue();
         

            return model;
        }
        public TopTenViewModel LoadDataValueAndCount()
        {

            var model = new TopTenViewModel();

          //  model.ValueCount = _CountryRepository.LoadItemsDataCategory();
          
            return model;
        }
    }
}
