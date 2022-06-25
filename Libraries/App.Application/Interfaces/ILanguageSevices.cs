using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Application.ViewModels;
using App.Application.Model;
using System.Threading.Tasks;
using System.Linq.Expressions;
using App.Domain.Interfaces;
using System;
using App.Domain.Model;

namespace App.Application.Interfaces
{
    public interface ILanguageSevices
    {
        Task<LanguageModel> GetById(int id);
        Task<int> GetByLanguageCulture(string id);
        Task<LanguageModel> GetLanguageByLanguageCulture(string id);
        Task Save(LanguageModel model);
        Task<bool> ChangeStatus(int id, bool status);
        Task<bool> Delete(int id);
        Task<IList<LocalizedProperty>> GetAllLocalizedProperties();
        Task<IList<Language>> GetAllLanguage();
        Task<IList<LanguageModel>> GetListLanguage();
        Task<Tuple<IList<LanguageViewModel>, int>> LoadData(string Search = null, int StatusId = 0,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);

        Task SaveLocalizedValue<T>(T entity,
           Expression<Func<T, string>> keySelector,
           string localeValue,
           int languageId) where T : BaseEntity, ILocalizedEntity;

        //  IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup);
        Task<TPropType> GetLocalized<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
            int? languageId = null, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where TEntity : BaseEntity, ILocalizedEntity;

        Task<IList<BaseViewModel>> GeLanguageList(string selectTitle);

    }
}