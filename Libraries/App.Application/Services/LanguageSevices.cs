using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using App.Application.Interfaces;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Model;
using AutoMapper;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using App.Application.Caching;
using System.Net;
using Microsoft.Extensions.Configuration;
using App.Domain.Model;

using App.Application.FrameWork;
using App.Application.Framwork;

namespace App.Application.Services
{
    public class LanguageSevices : ILanguageSevices
    {

        public readonly ILanguageRepository _LanguageRepository;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IConfiguration _Configuration;
        public LanguageSevices(ILanguageRepository LanguageRepository, ICacheManager cacheManager,
             IMapper mapper, IConfiguration configuration)
        {
            _LanguageRepository = LanguageRepository;

            _mapper = mapper;
            _cacheManager = cacheManager;
            _Configuration = configuration;
        }

        public async Task<LanguageModel> GetById(int id)
        {
            var obj = await _LanguageRepository.GetItemById(id);

            var model = _mapper.Map<LanguageModel>(obj);

            return model;
        }
        public async Task<LanguageModel> GetLanguageByLanguageCulture(string id)
        {
            var obj = await _LanguageRepository.GetLanguageByLanguageCulture(id);

            var model = _mapper.Map<LanguageModel>(obj);

            return model;
        }
        public async Task<int> GetByLanguageCulture(string id)
        {
            var obj = await _LanguageRepository.GetByLanguageCulture(id);

            return obj;
        }
        public async Task Save(LanguageModel model)
        {
            var obj = new Language();
            if (model.Id > 0)
                obj = await _LanguageRepository.GetItemById(model.Id);

            _mapper.Map(model, obj);

            await _LanguageRepository.SaveItem(obj);

            var CacheKey = string.Format(ModelCacheEventConsumer.All_Langauge_KEY);
            _cacheManager.Remove(CacheKey);

        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            return await _LanguageRepository.ChangeStatusItem(id, status);
        }

        public async Task<bool> Delete(int id)
        {
            await _LanguageRepository.DeleteItem(id);

            return true;
        }
        public async Task<Tuple<IList<LanguageViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _LanguageRepository.LoadItemsData(Search, StatusId,
                 jtStartIndex, jtPageSize, order, orderDir);


            var list = await data.Item1.SelectAwait(async obj =>
            {
                var model = _mapper.Map<LanguageViewModel>(obj);
                model.Name = await GetLocalized(obj, entity => entity.Name);

                return model;
            }).ToListAsync();

            return new Tuple<IList<LanguageViewModel>, int>(list, data.Item2);
        }

        //
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        public virtual async Task<TPropType> GetLocalized<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
           int? languageId = null, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
           where TEntity : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!(keySelector.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var result = default(TPropType);
            var resultStr = string.Empty;

            var localeKeyGroup = entity.GetType().Name;
            var localeKey = propInfo.Name;

            if (!languageId.HasValue)
                languageId = CommonHelper.WorkingLanguage.Id;

            if (languageId > 0)
            {
                //ensure that we have at least two published languages
                var loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    var totalPublishedLanguages = (await _LanguageRepository.GetAllLanguage()).Count;
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                //localized value
                if (loadLocalizedValue)
                {
                    resultStr = GetLocalizedValue(languageId.Value, entity.Id, localeKeyGroup, localeKey).Result;
                    if (!string.IsNullOrEmpty(resultStr))
                        result = To<TPropType>(resultStr);
                }
            }

            //set default value if required
            if (!string.IsNullOrEmpty(resultStr) || !returnDefaultValue)
                return result;
            var localizer = keySelector.Compile();
            result = localizer(entity);

            return result;
        }
        public virtual async Task<string> GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey)
        {
            var CacheKey = string.Format(ModelCacheEventConsumer.LocalizedProperty_KEY, languageId, entityId, localeKeyGroup, localeKey);

            return await _cacheManager.GetAsync(CacheKey, async () =>
            {
                var source = (await GetAllLocalizedProperties()).AsQueryable();

                var query = from lp in source
                            where lp.LanguageId == languageId &&
                                  lp.EntityId == entityId &&
                                  lp.LocaleKeyGroup == localeKeyGroup &&
                                  lp.LocaleKey == localeKey
                            select lp.LocaleValue;

                //little hack here. nulls aren't cacheable so set it to ""
                var localeValue = query.FirstOrDefault() ?? string.Empty;

                return localeValue;
            });
        }
        public virtual async Task<IList<LocalizedProperty>> GetAllLocalizedProperties()
        {
            var CacheKey = string.Format(ModelCacheEventConsumer.Alllocalized_KEY);

            return await _cacheManager.GetAsync(CacheKey, async () =>
            {
                var source = await _LanguageRepository.GetAllLocalizedProperties();

                return source;
            });
        }
        public async Task<IList<Language>> GetAllLanguage()
        {
            var CacheKey = string.Format(ModelCacheEventConsumer.All_Langauge_KEY);

            return await _cacheManager.GetAsync(CacheKey, async () =>
            {
                var list = await _LanguageRepository.GetAllLanguage();

                return list;
            });

        }

        public async Task<IList<BaseViewModel>> GeLanguageList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _LanguageRepository.GetAllLanguage();
            result.AddRange(listLookup.Select(s => new BaseViewModel()
            {
                Id = s.Id,
                Name = s.Name,
            }));
            return result;
        }

        public async Task<IList<LanguageModel>> GetListLanguage()
        {
            var CacheKey = string.Format(ModelCacheEventConsumer.All_List_Langauge_KEY);

            return await _cacheManager.Get(CacheKey, async () =>
            {
                var data = await _LanguageRepository.GetAllLanguage();

                var list = data.Select(obj =>
                {
                    var model = _mapper.Map<LanguageModel>(obj);

                    return model;
                });
                return list.ToList();
            });
        }
        public virtual async Task SaveLocalizedValue<T>(T entity,
          Expression<Func<T, string>> keySelector,
          string localeValue,
          int languageId) where T : BaseEntity, ILocalizedEntity
        {
            #region Clear Cach
            try
            {
                var CacheKey = string.Format(ModelCacheEventConsumer.Alllocalized_KEY);
                _cacheManager.Remove(CacheKey);

                var localeKeyGroup = entity.GetType().Name;
                if (!(keySelector.Body is MemberExpression member))
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a property.",
                        keySelector));
                }

                var propInfo = member.Member as PropertyInfo;
                var localeKey = propInfo.Name;

                var CacheKey2 = string.Format(ModelCacheEventConsumer.LocalizedProperty_KEY, languageId,
                    entity.Id, localeKeyGroup, localeKey);

                _cacheManager.Remove(CacheKey2);

               
            }
            catch (Exception ex) { }

            #endregion

            await _LanguageRepository.SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }
    }
}
