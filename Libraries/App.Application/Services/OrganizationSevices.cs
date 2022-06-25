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
    public class OrganizationSevices : IOrganizationSevices
    {
        public readonly IOrganizationRepository _OrganizationRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAttachmentService _attachmentService;
        public OrganizationSevices(IOrganizationRepository OrganizationRepository, IAttachmentService attachmentService,
             IMapper mapper
            , ILanguageSevices languageSevices, 
            IHttpContextAccessor httpContextAccessor, ILocalizedModelFactory localizedModelFactory)
        {
            _OrganizationRepository = OrganizationRepository;
            _languageSevices = languageSevices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizedModelFactory = localizedModelFactory;
            _attachmentService = attachmentService;
        }
        public async Task<OrganizationModel>  GetById(int id)
        {
            var obj = new Organization();
            if (id > 0)
                obj = await _OrganizationRepository.GetItemById(id);

            var model = _mapper.Map<OrganizationModel>(obj);
            Action<OrganizationLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _languageSevices.GetLocalized(obj, entity => entity.Name, languageId, false, false).Result;
            };
            model.Locales =await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task Save(OrganizationModel model)
        {
            var obj = new Organization();
            if (model.Id > 0)
                obj = await _OrganizationRepository.GetItemById(model.Id);
           
            _mapper.Map(model, obj);

            await _OrganizationRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _OrganizationRepository.ChangeStatusItem(id, status);
        }
        public async Task<bool> Delete(int id)
        {
            return await _OrganizationRepository.DeleteItem(id);
        }
            
        public async Task<Tuple<IList<OrganizationViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _OrganizationRepository.LoadItemsData(Search, StatusId,
               jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<OrganizationViewModel>(obj);
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                return model;
            }).ToListAsync();

            return new Tuple<IList<OrganizationViewModel>, int>(list, data.Item2);
        }
        public async Task UpdateLocales(Organization Organization, OrganizationModel model)
        {
            foreach (var localized in model.Locales)
            {
              await  _languageSevices.SaveLocalizedValue(Organization,
                     x => x.Name,
                     localized.Name,
                     localized.LanguageId);
            }
        }
    }
}
