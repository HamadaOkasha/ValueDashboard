using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;

using AutoMapper;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class MessagesTemplateServices : IMessagesTemplateServices
    {
        private readonly IMapper _mapper;
        private readonly IMessagesTemplateRepository _messagesTemplateRepository;
        private readonly ILanguageSevices _languageSevices;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ILocaleStringResourceSevices _localeStringResourceSevices;
        public MessagesTemplateServices(IMessagesTemplateRepository messagesTemplateRepository, IMapper mapper
            , ILanguageSevices languageSevices,
            ILocaleStringResourceSevices localeStringResourceSevices
            , ILocalizedModelFactory localizedModelFactory)
        {
            _messagesTemplateRepository = messagesTemplateRepository;
            _mapper = mapper;
            _languageSevices = languageSevices;
            _localizedModelFactory = localizedModelFactory;
            _localeStringResourceSevices = localeStringResourceSevices;
        }
        public async Task<MessagesTemplateModel> GetMessagesTemplateById(int id)
        {
            var obj = new MessageTemplate();
            if (id > 0)
                obj = await _messagesTemplateRepository.GetMessagesTemplateById(id);

            var model = _mapper.Map<MessagesTemplateModel>(obj);

            Action<MessagesTemplateLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Title = await _languageSevices.GetLocalized(obj, entity => entity.Title, languageId, false, false);
                locale.Description = await _languageSevices.GetLocalized(obj, entity => entity.Description, languageId, false, false);
            };
            model.Locales = await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        public async Task<MessagesTemplateModel> GetMessagesBodyById(int id)
        {
            var obj = new MessageTemplate();
            if (id > 0)
                obj = await _messagesTemplateRepository.GetMessagesTemplateById(id);

            var model = _mapper.Map<MessagesTemplateModel>(obj);

            //if (obj.IsActive == true)
            //    model.Description = await _languageSevices.GetLocalized(obj, entity => entity.Description, CommonHelper.LanguageId, false, false);

            return model;
        }
        //public async Task UpdateLocales(MessageTemplate country, MessagesTemplateModel model)
        //{
        //    foreach (var localized in model.Locales)
        //    {
        //        await _languageSevices.SaveLocalizedValue(country,
        //             x => x.Title,
        //             localized.Title,
        //             localized.LanguageId);

        //        await _languageSevices.SaveLocalizedValue(country,
        //             x => x.Description,
        //             localized.Description,
        //             localized.LanguageId);
        //    }
        //}
        public async Task SaveMessagesTemplate(MessagesTemplateModel model)
        {
            var obj = new MessageTemplate();
            if (model.Id > 0)
                obj = await _messagesTemplateRepository.GetMessagesTemplateById(model.Id);

            _mapper.Map(model, obj);

            if (model.Id > 0)
                obj.IsActive = true;

            await _messagesTemplateRepository.SaveMessagesTemplate(obj);

         //   await UpdateLocales(obj, model);
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _messagesTemplateRepository.ChangeStatus(id, status);
        }
        public async Task<Tuple<IList<MessagesTemplateViewModel>, int>> LoadMessagesTemplates(int TypeId, string Search,
             int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {

            var data = await _messagesTemplateRepository.LoadMessagesTemplates(TypeId, Search, jtStartIndex,
                jtPageSize, order, orderDir);

            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<MessagesTemplateViewModel>(obj);

                return model;
            });

            return new Tuple<IList<MessagesTemplateViewModel>, int>(list.ToList(), data.Item2);
        }

        public async Task<MessagesTemplateModel> PopulateForgetEmail(string Name = null, string Url = null)
        {
            var item = await GetMessagesBodyById((int)EnumMessagesTemplate.ForgetPassword);

            if (!string.IsNullOrEmpty(item.Description))
            {
                item.Description = item.Description.Replace("{CustomerName}", Name);
                item.Description = item.Description.Replace("{Url}", "<a href='" + HtmlEncoder.Default.Encode(Url) + "'>" + _localeStringResourceSevices.GetResource("ClickHere") + "</a>.");
            }

            return item;
        }
        public MessagesTemplateModel GetMessageBody(int id, string Name = null, int orderNo = 0)
        {
            var obj = _messagesTemplateRepository.GetMessagesTemplateById(id);

            var model = _mapper.Map<MessagesTemplateModel>(obj);
            model.Subject = model.Title;
            model.Body = model.Description;

            model.Body = model.Body.Replace("{Name}", Name);
            model.Body = model.Body.Replace("{OrderNumber}", orderNo.ToString());

            return model;

        }
        public async Task<Tuple<IList<MessagesTemplateViewModel>, int>> LoadData( string Search = null,
        int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            

            var data = await _messagesTemplateRepository.LoadItemsData(Search,
                 jtStartIndex, jtPageSize, order, orderDir);


            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<MessagesTemplateViewModel>(obj);

                return model;
            });

            return new Tuple<IList<MessagesTemplateViewModel>, int>(list.ToList(), data.Item2);
        }
        public async Task Save(MessagesTemplateModel model)
        {
            var obj = new MessageTemplate();
            if (model.Id > 0)
                obj = await _messagesTemplateRepository.GetItemById(model.Id);

            _mapper.Map(model, obj);
          
            await _messagesTemplateRepository.SaveItem(obj);
        }
        public async Task<MessagesTemplateModel> GetById(int id)
        {
            var obj = new MessageTemplate();

            if (id > 0)
                obj = await _messagesTemplateRepository.GetItemById(id);

            var model = _mapper.Map<MessagesTemplateModel>(obj);
            Action<MessagesTemplateLocalizedModel, int> localizedModelConfiguration = null;
            localizedModelConfiguration = (Locales, languageId) =>
            {
                Locales.Title =    _languageSevices.GetLocalized(obj, entity => entity.Title).Result;
                Locales.Description = _languageSevices.GetLocalized(obj, entity => entity.Description, languageId, false, false).Result;
            };
            model.Locales = await _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            return model;
        }
    }
}
