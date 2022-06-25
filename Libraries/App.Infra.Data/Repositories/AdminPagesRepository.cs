
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

    public class AdminPagesRepository : IAdminPagesRepository
    {
        public AppDbContext _context;
        public AdminPagesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AdminPages> GetItemById(int Id)
        {
            var item = await _context.AdminPages.Where(s => s.Id == Id).FirstOrDefaultAsync();

            return item;
        }
        public async Task SaveItem(AdminPages obj)
        {
            if (obj.Id == 0)
            {
                var lastItem =await  _context.AdminPages
                    .OrderByDescending(s => s.Id).FirstOrDefaultAsync() ;
                if (lastItem != null)
                {
                    var lastId = lastItem.Id + 1;
                    obj.Id = lastId;
                }
               else
                    obj.Id = 1;


                await _context.AdminPages.AddAsync(obj);
            }
            else
            {
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Tuple<IList<AdminPages>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var AllListCount = 0;
            var dataQuery = _context.AdminPages.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = dataQuery.Where(x => x.PageTitle.Contains(Search));
            }

            var result = dataQuery.Select(s => s).AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.PageTitle) : result.OrderByDescending(x => x.PageTitle);
                        break;
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.ParentId) : result.OrderByDescending(x => x.ParentId);
                        break;
                    case "3":
                        result = orderDir == "asc" ? result.OrderBy(x => x.DisplayOrder) : result.OrderByDescending(x => x.DisplayOrder);
                        break;
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.DisplayOrder);
            }

            AllListCount = await dataQuery.CountAsync().ConfigureAwait(false);

            return new Tuple<IList<AdminPages>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync().ConfigureAwait(false), AllListCount);
        }
    }
}
