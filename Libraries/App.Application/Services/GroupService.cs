using AutoMapper;
using App.Application.Caching;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Application.FrameWork;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;
        private readonly ICacheManager _cacheManager;
        public GroupService(IGroupRepository groupRepository, 
            IMapper mapper, ICacheManager cacheManager)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }
        public async Task<GroupModel> GetById(string id)
        {
            var obj = new AspNetRoles();
            if (!string.IsNullOrEmpty(id))
                obj = await _groupRepository.GetItemById(id);

            var model = _mapper.Map<GroupModel>(obj);

            model.AdminPagesIds = obj.RolePages.Select(g => g.PageId.ToString()).ToArray();
         
            return model;
        }
        public async Task<bool> Save(GroupModel model)
        {
            var obj = new AspNetRoles();

            if (await _groupRepository.CheckExisting(model.Id, model.Name))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(model.Id))
            {
                obj = await _groupRepository.GetItemById(model.Id);

                //remove cache grop
                var menuCacheKey = string.Format(ModelCacheEventConsumer.MENU_KEY, model.Name);
                _cacheManager.RemoveByPattern(menuCacheKey);
            }
            _mapper.Map(model, obj);

            if (model.AdminPagesIds != null)
            {
                var listAdminPagesIds = model.AdminPagesIds
                                        .Select(g => new RolePages()
                                        {
                                            PageId = Convert.ToInt32(g),
                                            RoleId = obj.Id,
                                        }).ToList();
                obj.RolePages = listAdminPagesIds;
            }
            obj.NormalizedName = obj.Name.ToUpper();
            await _groupRepository.SaveItem(obj);

            return true;
        }
        public async Task<Tuple<IList<GroupViewModel>, int>> LoadData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var data = await _groupRepository.LoadItemsData(Search, jtStartIndex,
                jtPageSize, order, orderDir);


            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<GroupViewModel>(obj);

                return model;
            });

            return new Tuple<IList<GroupViewModel>, int>(list.ToList(), data.Item2);
        }
        public async Task<IList<BaseViewModel>> GeAdminPagesList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _groupRepository.GeAdminPagesList();
            result.AddRange(listLookup.Select(s => new BaseViewModel()
            {
                Id = s.Id,
                Name = s.PageTitle,
            }));
            return result;
        }
        public async Task<bool> Delete(string id)
        {
            await _groupRepository.DeleteItem(id);
            return true;
        }
        public async Task<IList<BaseViewModel>> GeCategoryList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Id = 0,
                    Name = selectTitle,
                });

            var listLookup = await _groupRepository.GeCountryList();
            result.AddRange(listLookup.Select(s => new BaseViewModel()
            {
                Id = s.Id,
                Name = s.Name,
            }));
            return result;
        }
        public async Task<IList<BaseViewModel>> GeRolesList(string selectTitle)
        {
            List<BaseViewModel> result = new List<BaseViewModel>();
            if (!string.IsNullOrEmpty(selectTitle))
                result.Add(new BaseViewModel()
                {
                    Name = selectTitle,
                });

            var listLookup = await _groupRepository.GeRolesList();
            result.AddRange(listLookup.Select(s => new BaseViewModel()
            {
                IdString = s.Name,
                Name = s.Name,
            }));
            return result;
        }
    }
}
