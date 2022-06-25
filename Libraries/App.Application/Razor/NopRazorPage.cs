
using App.Application.Interfaces;
using System;
using App.Application.Services;
using App.Application.Localization;
using App.Application.ViewModels;

namespace App.Application.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class NopRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private ILocaleStringResourceSevices _localizationService;

        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer Translate
        {
            get
            {
                if (_localizationService == null)
                    _localizationService = EngineContext.Current.Resolve<ILocaleStringResourceSevices>();

                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = _localizationService.GetResource(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }
    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class NopRazorPage : NopRazorPage<dynamic>
    {
    }
}