using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Web;

namespace App.Application.TagHelpers
{
    /// <summary>
    /// Utility class that allows serialisation of .NET resource files (.resx)
    /// into different formats
    /// </summary>
    internal static class ResourceGroupExtensions
    {
        /// <summary>
        /// Converts the source data to a Javascript variable
        /// </summary>
        /// <param name="resources">The record to convert</param>
        /// <returns>A valid Javascript object</returns>
        internal static string ToJavascript(this IEnumerable<ResourceGroup> resources)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var Resources = ");

            ExpandoObject uiCaptions = new ExpandoObject();

            // Get the fields
            foreach (ResourceGroup fieldGroup in resources)
                ((IDictionary<string, object>)uiCaptions)[fieldGroup.Name] =
                    fieldGroup.Entries.ToDictionary(x => x.Name.ToString(), x => x.Value);

            string serialized = JsonSerializer.Serialize(uiCaptions);
            sb.Append(serialized);
            sb.Append(";");

            return sb.ToString();
        }
    }
}