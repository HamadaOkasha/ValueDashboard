using App.Application.Interfaces;
using App.Application.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Validator
{
    //public class LanguageValidatorDaynmicTranslate : AbstractValidator<LanguageModel>
    //{
    //    public LanguageValidatorDaynmicTranslate(ILocaleStringResourceSevices localizationService)
    //    {
    //        RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("FieldRequired"));
    //        RuleFor(x => x.DisplayOrder).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
    //    }
    //}
}
