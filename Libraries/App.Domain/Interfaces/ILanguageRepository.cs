using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface ILanguageRepository
    {
        Task<bool> DeleteItem(int id);
        Task<Language> GetItemById(int id);
        Task<int> GetByLanguageCulture(string id);
        Task<Language> GetLanguageByLanguageCulture(string id);
        Task SaveItem(Language obj);
        Task<bool> ChangeStatusItem(int id, bool status);
        Task<Tuple<IList<Language>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null);

        Task<IList<Language>> GetAllLanguage();
        Task<IList<LocalizedProperty>> GetAllLocalizedProperties();

        Task SaveLocalizedValue<T>(T entity,
           Expression<Func<T, string>> keySelector,
           string localeValue,
           int languageId) where T : BaseEntity, ILocalizedEntity;
        Task SaveLocalizedValue<T, TPropType>(T entity,
                   Expression<Func<T, TPropType>> keySelector,
                   TPropType localeValue,
                   int languageId) where T : BaseEntity, ILocalizedEntity;
    }
}
