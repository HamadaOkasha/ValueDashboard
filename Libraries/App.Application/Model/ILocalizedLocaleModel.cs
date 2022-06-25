using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Model
{
    public interface ILocalizedLocaleModel
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        int LanguageId { get; set; }
    }
}
