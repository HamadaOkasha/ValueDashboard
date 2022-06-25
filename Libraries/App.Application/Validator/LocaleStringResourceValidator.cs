using App.Application.Interfaces;
using App.Application.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Validator
{
    public class LocaleStringResourceValidator : AbstractValidator<LocaleStringResourceModel>
    {
        public LocaleStringResourceValidator(ILocaleStringResourceSevices localizationService)
        {
            RuleFor(x => x.ResourceName).NotEmpty().WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.ResourceValue).NotEmpty().WithMessage(localizationService.GetResource("FieldRequired"));
        }
    }
}
