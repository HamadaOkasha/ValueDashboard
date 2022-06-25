using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using App.Application.FrameWork;
using App.Application.Interfaces;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Model;
using App.Domain.Model;
using AutoMapper;
using App.Application.Framwork;

namespace App.Application.Services
{
    public class LookupSevices : ILookupSevices
    {
        public readonly ILookupRepository _lookupRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageSevices _languageSevices;
        public LookupSevices(ILookupRepository lookupRepository,
           IMapper mapper, ILanguageSevices languageSevices)
        {
            _lookupRepository = lookupRepository;
            _mapper = mapper;
            _languageSevices = languageSevices;
        }
        public async Task<IList<BaseViewModel>> GetParentList(string selectTitle = null)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetParentList();

            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.PageTitle);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());


            return result;
        }
        
        public async Task<IList<BaseViewModel>> GetCountryList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetCountryList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        } 
        public async Task<IList<BaseViewModel>> GetCategoryList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetCategoryList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        } 
        public async Task<IList<BaseViewModel>> GetSectorList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetSectorList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        }
        public async Task<IList<BaseViewModel>> GetOrganizationList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetOrganizationList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        }  
        public async Task<IList<BaseViewModel>> GetValueList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetValueList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        } 
        public async Task<IList<BaseViewModel>> GetResearchValueList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetResearchValueList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        } 
        public async Task<IList<BaseViewModel>> GetVersionList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _lookupRepository.GetVersionList();
            result.AddRange(await listLookup.SelectAwait(async obj =>
            {
                var model = new BaseViewModel();
                model.Name = await _languageSevices.GetLocalized(obj, entity => entity.Name);
                model.Id = obj.Id;
                return model;
            }).ToListAsync());

            return result;
        }

    }
}
