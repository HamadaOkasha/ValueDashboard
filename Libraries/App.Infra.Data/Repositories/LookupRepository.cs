
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Data.Repositories
{

    public class LookupRepository : ILookupRepository
    {
        public AppDbContext _context;
        public LookupRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IList<AdminPages>> GetParentList()
        {
            return await _context.AdminPages.Where(s => s.ParentId == 0).ToListAsync();
        }
        public async Task<IList<Country>> GetCountryList()
        {
            return await _context.Country.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();
             
        }
        public async Task<IList<Category>> GetCategoryList()
        {
            return await _context.Category.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Sector>> GetSectorList()
        {
            return await _context.Sector.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Organization>> GetOrganizationList()
        {
            return await _context.Organization.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Value>> GetValueList()
        {
            return await _context.Value.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<ResearchValue>> GetResearchValueList()
        {
            return await _context.ResearchValue.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<AlrowadVersion>> GetVersionList()
        {
            return await _context.AlrowadVersion.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }

    }
}
