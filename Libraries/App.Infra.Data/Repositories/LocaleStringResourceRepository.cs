
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Data.Repositories
{

    public class LocaleStringResourceRepository : ILocaleStringResourceRepository
    {
        public AppDbContext _context;
        public LocaleStringResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LocaleStringResource> GetItemById(int id)
        {
            var item = await _context.LocaleStringResource.Where(s=> s.Id == id).FirstOrDefaultAsync();
            return item;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _context.LocaleStringResource.Where(s => s.Id == id).FirstOrDefaultAsync();
            _context.LocaleStringResource.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<LocaleStringResource>> GetAllResource(int LanguageId = 1)
        {
            
            var item = await _context.LocaleStringResource.Where(s=> s.LanguageId == LanguageId).ToListAsync();
            return item;
        }

        public async Task<bool> CheckResourceName(string ResourceName, int currentId  ,  int langId = 1)
        {
            var item = await _context.LocaleStringResource.Where(s => s.ResourceName == ResourceName && s.Id != currentId && s.LanguageId == langId).AnyAsync();
            return item;
        }

        public async Task SaveItem(LocaleStringResource obj)
        {
            if (obj.Id == 0)
            {
               await _context.LocaleStringResource.AddAsync(obj);
            }
            else
            {
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
           await _context.SaveChangesAsync();
        }
        public async Task<Tuple<IList<LocaleStringResource>,int>> LoadItemsData(string ResourceName, string ResourceValue, int LanguageId, int jtStartIndex = 0,
         int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var dataQuery = _context.LocaleStringResource.AsQueryable();

            if (!string.IsNullOrEmpty(ResourceName))
            {
                dataQuery = dataQuery.Where(x => x.ResourceName.ToLower().Contains(ResourceName.ToLower()));
            }
           
            if (!string.IsNullOrEmpty(ResourceValue))
            {
                dataQuery = dataQuery.Where(x => x.ResourceValue.ToLower().Contains(ResourceValue.ToLower()));
            }
            if (LanguageId > 0)
            {
                dataQuery = dataQuery.Where(x => x.LanguageId == LanguageId);
            }
            var result = dataQuery.Select(s => s).AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.ResourceName) : result.OrderByDescending(x => x.ResourceName);
                        break;
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.ResourceValue) : result.OrderByDescending(x => x.ResourceValue);
                        break;
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            }
            int AllListCount = await dataQuery.CountAsync();

            return new Tuple<IList<LocaleStringResource>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
        }
        public async Task<IList<LocaleStringResource>> GetAllResourceMobile(int LanguageId = 1)
        {
            var item = await _context.LocaleStringResource.Where(s => s.LanguageId == LanguageId
            && s.ResourceName.StartsWith("mobile.")).ToListAsync();
            return item;
        }

    }
}
