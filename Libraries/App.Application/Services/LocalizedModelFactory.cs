using App.Application.Interfaces;
using App.Application.Model;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public partial class LocalizedModelFactory : ILocalizedModelFactory
    {
        #region Fields

        private readonly ILanguageRepository _languageRepository;

        #endregion

        #region Ctor

        public LocalizedModelFactory(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare localized model for localizable entities
        /// </summary>
        /// <typeparam name="T">Localized model type</typeparam>
        /// <param name="configure">Model configuration action</param>
        /// <returns>List of localized model</returns>
        public virtual async Task<IList<T>> PrepareLocalizedModels<T>(Action<T, int> configure = null) where T : ILocalizedLocaleModel
        {
            //get all available languages
            var availableLanguages = await _languageRepository.GetAllLanguage();

            //prepare models
            var localizedModels = availableLanguages.Select(language =>
            {
                //create localized model
                var localizedModel = Activator.CreateInstance<T>();

                //set language
                localizedModel.LanguageId = language.Id;
                //invoke the model configuration action
                configure?.Invoke(localizedModel, localizedModel.LanguageId);

                return localizedModel;
            }).ToList();

            return localizedModels;
        }

        #endregion
    }
}
