using App.Application.Interfaces;
using App.Application.Model;
using App.Application.Services;
using App.Domain.Interfaces;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace App.Application.Framwork
{
    public static class HtmlExtensions
    {
        public static async Task<IHtmlContent> LocalizedEditor<T, TLocalizedModelLocal>(this IHtmlHelper<T> helper,
          string name,
          Func<int, HelperResult> localizedTemplate,
          Func<T, HelperResult> standardTemplate,
          bool ignoreIfSeveralStores = false, string cssClass = "")
          where T : ILocalizedModel<TLocalizedModelLocal>
          where TLocalizedModelLocal : ILocalizedLocaleModel
        {
            var tabStrip = new StringBuilder();
            var cssClassWithSpace = !string.IsNullOrEmpty(cssClass) ? " " + cssClass : null;
            tabStrip.AppendLine($"<div id=\"{name}\" class=\"nav-tabs-custom nav-tabs-localized-fields{cssClassWithSpace}\">");

            //render input contains selected tab name
            var tabNameToSelect = GetSelectedTabName(helper, name);
            var selectedTabInput = new TagBuilder("input");
            selectedTabInput.Attributes.Add("type", "hidden");
            selectedTabInput.Attributes.Add("id", $"selected-tab-name-{name}");
            selectedTabInput.Attributes.Add("name", $"selected-tab-name-{name}");
            selectedTabInput.Attributes.Add("value", tabNameToSelect);
            tabStrip.AppendLine(selectedTabInput.RenderHtmlContent());

            tabStrip.AppendLine("<ul class=\"nav nav-tabs\">");

            var _localizer = EngineContext.Current.Resolve<ILocaleStringResourceSevices>();
            //default tab
            var standardTabName = $"{name}-standard-tab";
            var standardTabSelected = string.IsNullOrEmpty(tabNameToSelect) || standardTabName == tabNameToSelect;
            tabStrip.AppendLine(string.Format("<li{0}>", standardTabSelected ? " class=\"nav-item  active\"" : " class=\"nav-item \""));
            tabStrip.AppendLine($"<a  class=\"nav-link active\" data-tab-name=\"{standardTabName}\" href=\"#{standardTabName}\"  data-toggle=\"tab\">{_localizer.GetResource("Standard")}</a>");
            tabStrip.AppendLine("</li>");

            var languageService = EngineContext.Current.Resolve<ILanguageRepository>();
            //    var urlHelper = EngineContext.Current.Resolve<IUrlHelperFactory>().GetUrlHelper(helper.ViewContext);

            foreach (var locale in helper.ViewData.Model.Locales)
            {
                //languages
                var language = await languageService.GetItemById(locale.LanguageId);
                if (language == null)
                    throw new Exception("Language cannot be loaded");

                var localizedTabName = $"{name}-{language.Id}-tab";
                tabStrip.AppendLine(string.Format("<li{0}>", localizedTabName == tabNameToSelect ? " class=\"nav-item  active\"" : " class=\"nav-item \""));
                tabStrip.AppendLine($"<a class=\"nav-link\"  data-tab-name=\"{localizedTabName}\" href=\"#{localizedTabName}\" data-toggle=\"tab\">{WebUtility.HtmlEncode(language.Name)}</a>");

                tabStrip.AppendLine("</li>");
            }
            tabStrip.AppendLine("</ul>");

            //default tab
            tabStrip.AppendLine("<div class=\"tab-content\">");
            tabStrip.AppendLine(string.Format("<div class=\"tab-pane{0}\" id=\"{1}\">", standardTabSelected ? " active" : null, standardTabName));
            tabStrip.AppendLine(standardTemplate(helper.ViewData.Model).ToHtmlString());
            tabStrip.AppendLine("</div>");

            for (var i = 0; i < helper.ViewData.Model.Locales.Count; i++)
            {
                //languages
                var language = await languageService.GetItemById(helper.ViewData.Model.Locales[i].LanguageId);
                if (language == null)
                    throw new Exception("Language cannot be loaded");

                var localizedTabName = $"{name}-{language.Id}-tab";
                tabStrip.AppendLine(string.Format("<div class=\"tab-pane{0}\" id=\"{1}\">", localizedTabName == tabNameToSelect ? " active" : null, localizedTabName));
                tabStrip.AppendLine(localizedTemplate(i).ToHtmlString());
                tabStrip.AppendLine("</div>");
            }
            tabStrip.AppendLine("</div>");
            tabStrip.AppendLine("</div>");

            //render tabs script
            var script = new TagBuilder("script");
            script.InnerHtml.AppendHtml("$(document).ready(function () {bindBootstrapTabSelectEvent('" + name + "', 'selected-tab-name-" + name + "');});");
            tabStrip.AppendLine(script.RenderHtmlContent());

            return new HtmlString(tabStrip.ToString());
        }
        public static string ToHtmlString(this IHtmlContent tag)
        {
            using var writer = new StringWriter();
            tag.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }
        public static string GetSelectedTabName(this IHtmlHelper helper, string dataKeyPrefix = null)
        {
            //keep this method synchronized with
            //"SaveSelectedTab" method of \Area\Admin\Controllers\BaseAdminController.cs
            var tabName = string.Empty;
            var dataKey = "nop.selected-tab-name";
            if (!string.IsNullOrEmpty(dataKeyPrefix))
                dataKey += $"-{dataKeyPrefix}";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.TempData[dataKey].ToString();

            return tabName;
        }

    }
}
