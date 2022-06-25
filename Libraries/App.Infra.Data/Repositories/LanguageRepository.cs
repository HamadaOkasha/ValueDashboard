using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

namespace App.Infra.Data.Repositories
{
    //Repository<Language>,
    public class LanguageRepository : ILanguageRepository
    {
        public AppDbContext _context;
        public LanguageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Language> GetItemById(int Id)
        {
            return await _context.Language
                .Where(x => x.Id == Id && !x.IsDeleted).FirstOrDefaultAsync();
        }
        public async Task<int> GetByLanguageCulture(string LanguageCulture)
        {
            var item = await _context.Language
                .Where(x => x.LanguageCulture == LanguageCulture).FirstOrDefaultAsync();

            if (item != null)
                return item.Id;
            else
                return 1;
        }
        public async Task<Language> GetLanguageByLanguageCulture(string LanguageCulture)
        {
            var item = await _context.Language
                .Where(x => x.LanguageCulture == LanguageCulture).FirstOrDefaultAsync();

            if (item != null)
                return item;
            else
                return await _context.Language.OrderBy(s => s.DisplayOrder).FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteItem(int Id)
        {
            var dbObj = await _context.Language.Where(x => x.Id == Id && !x.IsDeleted).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            dbObj.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task SaveItem(Language obj)
        {
            if (obj.Id == 0)
            {
                obj.IsDeleted = false;
                await _context.Language.AddAsync(obj);
            }
            else
            {
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ChangeStatusItem(int Id, bool status)
        {
            var dbObj = await _context.Language.Where(s => s.Id == Id).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            dbObj.IsPublish = status;
            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task SaveLocalizedValue<T>(T entity,
          Expression<Func<T, string>> keySelector,
          string localeValue,
          int languageId) where T : BaseEntity, ILocalizedEntity
        {
            await SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }
        public virtual async Task SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            if (!(keySelector.Body is MemberExpression member))
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var localeKeyGroup = entity.GetType().Name;
            var localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.Id, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var localeValueStr = To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    await DeleteLocalizedProperty(prop.Id);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    await UpdateLocalizedProperty(prop);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                    return;

                //insert
                prop = new LocalizedProperty
                {
                    EntityId = entity.Id,
                    LanguageId = languageId,
                    LocaleKey = localeKey,
                    LocaleKeyGroup = localeKeyGroup,
                    LocaleValue = localeValueStr
                };
                await InsertLocalizedProperty(prop);
            }
        }
        public async Task DeleteLocalizedProperty(int id)
        {
            var obj = await _context.LocalizedProperty.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Remove(obj);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLocalizedProperty(LocalizedProperty obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task InsertLocalizedProperty(LocalizedProperty obj)
        {
            await _context.LocalizedProperty.AddAsync(obj);
            await _context.SaveChangesAsync();
        }
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

        public virtual IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
                return new List<LocalizedProperty>();

            var query = from lp in _context.LocalizedProperty
                        orderby lp.Id
                        where lp.EntityId == entityId &&
                              lp.LocaleKeyGroup == localeKeyGroup
                        select lp;

            var props = query.ToList();

            return props;
        }
        public async Task<IList<Language>> GetAllLanguage()
        {
            return await _context.Language.Where(s => s.IsPublish && !s.IsDeleted).ToListAsync()
                .ConfigureAwait(false);
        }


       
        public async Task<IList<LocalizedProperty>> GetAllLocalizedProperties()
        {
            var query = await _context.LocalizedProperty.ToListAsync();

            return query;
        }
        public async Task<Tuple<IList<Language>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var AllListCount = 0;
            var dataQuery = _context.Language
                .Where(x => x.IsDeleted != true &&
                (StatusId == 0 || (StatusId == 1 && x.IsPublish == true)
                || (StatusId == 2 && x.IsPublish == false)) && !x.IsDeleted);

            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = dataQuery.Where(x => x.Name.Contains(Search));
            }

            var result = dataQuery.AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Name) : result.OrderByDescending(x => x.Name);
                        break; case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.LanguageCulture) : result.OrderByDescending(x => x.LanguageCulture);
                        break;
                   
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.DisplayOrder) : result.OrderByDescending(x => x.DisplayOrder);
                        break;
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            }

            AllListCount = await dataQuery.CountAsync();

            return new Tuple<IList<Language>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
        }

    }
}
