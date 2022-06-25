using App.Application.Interfaces;
using App.Application.Model;

using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Validator
{
    public class AlrowadDataValidator : AbstractValidator<AlrowadDataModel>
    {
     
        public AlrowadDataValidator(ILocaleStringResourceSevices localizationService)
        {
          //  RuleFor(x => x.CountryId).NotEmpty().WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.CountryId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.SectorId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.OrganizationId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.ValueId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.ResearchValueId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
            RuleFor(x => x.AlrowadVersionId).InclusiveBetween(1, int.MaxValue).WithMessage(localizationService.GetResource("FieldRequired"));
        }
    }
}
