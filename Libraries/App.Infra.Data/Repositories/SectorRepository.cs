
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
 
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Model;
using Microsoft.AspNetCore.Http;
 
namespace App.Infra.Data.Repositories
{

    public class SectorRepository : ISectorRepository
    {
        public AppDbContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public SectorRepository(AppDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Sector> GetItemById(int Id)
        {
            return await _context.Sector
                .Where(x => x.Id == Id && !x.IsDeleted).FirstOrDefaultAsync();
        }
        public async Task<bool> ChangeStatusItem(int id, bool status)
        {
            var dbObj = await _context.Sector.
                Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            dbObj.IsPublish = status;
           await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteItem(int id)
        {
            var dbObj = await _context.Sector.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            dbObj.IsDeleted = true;
          await  _context.SaveChangesAsync();

            return true;
        }
        public async Task SaveItem(Sector obj)
        {
            if (obj.Id == 0)
            {
                obj.IsDeleted = false;
                obj.IsPublish = true;
              
                await _context.Sector.AddAsync(obj);
            }
            else
            {
                
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        } 
        public async Task<Tuple<IList<Sector>, int>> LoadItemsData(string Search, int StatusId,  int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0)
        {
           var AllListCount = 0;
            var dataQuery = _context.Sector
                .Where(x => x.IsDeleted != true);
            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = from c in dataQuery
                            join lp in _context.LocalizedProperty on c.Id equals lp.EntityId into p_lp
                            from lp in p_lp.DefaultIfEmpty()
                            where
                            c.Name.Contains(Search)
                            || (lp.LanguageId == languageId && lp.LocaleKeyGroup == "Donors" && lp.LocaleKey == "Name" && lp.LocaleValue.Contains(Search))
                            select c;
                dataQuery = dataQuery.Distinct();
            }
            
            

            var result = dataQuery.AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Name) : result.OrderByDescending(x => x.Name);
                        break; 
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            } 
                AllListCount =await dataQuery.CountAsync();
            return new Tuple<IList<Sector>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);

        }

        
    }
}
