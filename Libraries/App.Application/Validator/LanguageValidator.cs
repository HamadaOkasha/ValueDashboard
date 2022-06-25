using App.Application.Interfaces;
using App.Application.Model;

using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Validator
{
    public class LanguageValidator : AbstractValidator<LanguageModel>
    {
     
        public LanguageValidator(ILocaleStringResourceSevices localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.DisplayOrder).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
        }
    }
}
