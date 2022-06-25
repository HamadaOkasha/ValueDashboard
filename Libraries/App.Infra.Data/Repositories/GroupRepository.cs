
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace App.Infra.Data.Repositories
{

    public class GroupRepository : IGroupRepository
    {
        public AppDbContext _context;
        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AspNetRoles> GetItemById(string Id)
        {
            var item = await _context.AspNetRoles
                .Where(x => x.Id == Id)
                .Include(s => s.RolePages)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            return item;
        }
        public async Task<bool> CheckExisting(string id, string roleName)
        {
            var item = await _context.AspNetRoles
                .Where(x => x.Id != id && x.Name == roleName).AnyAsync();

            return item;
        }
        public async Task SaveItem(AspNetRoles obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.Id = Guid.NewGuid().ToString();
                await _context.AspNetRoles.AddAsync(obj);
            }
            else
            {
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveGroupPages(List<RolePages> list, string groupId)
        {
            var deletedItems = _context.RolePages
                      .Where(x => x.RoleId == groupId
                        && !list.Select(s => s.PageId).Contains(x.PageId));

            var effectItems = _context.RolePages
                  .Where(x => x.RoleId == groupId
                    && list.Select(s => s.PageId)
                    .Contains(x.PageId)).Select(s => s.PageId);

            _context.RolePages.RemoveRange(deletedItems);

            foreach (var item in list)
            {
                if (!effectItems.Contains(item.PageId))
                    await _context.RolePages.AddAsync(item);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Tuple<IList<AspNetRoles>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var AllListCount = 0;
            var dataQuery = _context.AspNetRoles.Where(s => s.Id != "1" && s.Id != "2").AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = dataQuery
                    .Where(x => x.Name.Contains(Search));
            }
            var result = dataQuery.Select(s => s).AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Name) : result.OrderByDescending(x => x.Name);
                        break;
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            }

            AllListCount = await dataQuery.CountAsync().ConfigureAwait(false);

            return new Tuple<IList<AspNetRoles>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync().ConfigureAwait(false), AllListCount);
        }
        public async Task<IList<AdminPages>> GeAdminPagesList()
        {
            return await _context.AdminPages.Where(s => !string.IsNullOrEmpty(s.PageUrl)).ToListAsync().ConfigureAwait(false);
        }
       
        public async Task<IList<Country>> GeCountryList()
        {
            return await _context.Country.Where(x => x.IsDeleted != true&& x.IsPublish == true).ToListAsync();
        }
        public async Task<IList<AspNetRoles>> GeRolesList()
        {
            return await _context.AspNetRoles.Where(s => s.Id != "1").ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteItem(string Id)
        {
            var dbObj = await _context.AspNetRoles.Where(x => x.Id == Id ).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            else
            {
                 _context.AspNetRoles.Remove(dbObj);
            }
           
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
