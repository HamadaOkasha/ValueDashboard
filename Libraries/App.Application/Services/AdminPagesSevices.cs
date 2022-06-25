using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using App.Application.Caching;
using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.Application.Framwork;

namespace App.Application.Services
{
    public class AdminPagesSevices : IAdminPagesSevices
    {
        private readonly ICacheManager _cacheManager;
        public readonly IAdminPagesRepository _adminPagesRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        private readonly ILanguageSevices _languageSevices;
        private readonly ILocalizedModelFactory _localizedModelFactory;

        public AdminPagesSevices(IAdminPagesRepository adminPagesRepository,
              ICacheManager cacheManager, ILocaleStringResourceSevices localeStringResourceSevices,
             IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
             ILanguageSevices languageSevices, ILocalizedModelFactory localizedModelFactory)
        {
            _adminPagesRepository = adminPagesRepository;
            _httpContextAccessor = httpContextAccessor;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _localeStringResourceSevices = localeStringResourceSevices;
            _languageSevices = languageSevices;
            _localizedModelFactory = localizedModelFactory;
        }
        public async Task<AdminPagesModel> GetById(int id)
        {
            var obj = new AdminPages();

            if (id > 0)
                obj = await _adminPagesRepository.GetItemById(id);

            var model = _mapper.Map<AdminPagesModel>(obj);

            Action<AdminPagesLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = async (locale, languageId) =>
            {
                //locale.PageTitle = await _languageSevices.GetLocalized(obj, entity => entity.PageTitle, languageId, false, false);
                locale.PageTitle = await _languageSevices.GetLocalized(obj, entity => entity.PageTitle,CommonHelper.WorkingLanguage.Id, false, false);
            };
            model.Locales = await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);


            return model;
        }
        public async Task Save(AdminPagesModel model)
        {
            var obj = new AdminPages();
            if (model.Id > 0)
                obj = await _adminPagesRepository.GetItemById(model.Id);

            _mapper.Map(model, obj);

            await _adminPagesRepository.SaveItem(obj);

            await UpdateLocales(obj, model);
        }
        public async Task UpdateLocales(AdminPages AdminPages, AdminPagesModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _languageSevices.SaveLocalizedValue(AdminPages,
                     x => x.PageTitle,
                     localized.PageTitle,
                     localized.LanguageId);
            }
        }
        public async Task<Tuple<IList<AdminPagesViewModel>, int>> LoadData(string Search = null,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _adminPagesRepository.LoadItemsData(Search,
                 jtStartIndex, jtPageSize, order, orderDir);

            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<AdminPagesViewModel>(obj);
                if (obj.ParentId > 0)
                {
                    var page = await _adminPagesRepository.GetItemById(obj.ParentId.Value);
                    model.ParentPageTitle = page.PageTitle;
                }
                else
                    model.ParentPageTitle = _localeStringResourceSevices.GetResource("NoParentPage");
                return model;

            }).ToListAsync();

            return new Tuple<IList<AdminPagesViewModel>, int>(list, data.Item2);
        }

    }
}
