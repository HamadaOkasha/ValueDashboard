
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
using Microsoft.Data.SqlClient;
//using Abp.Linq.Expressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;

namespace App.Infra.Data.Repositories
{

    public class CountryRepository : ICountryRepository
    {
        public AppDbContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public CountryRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Country> GetItemById(int Id)
        {
            return await _context.Country
                .Where(x => x.Id == Id && !x.IsDeleted).FirstOrDefaultAsync();
        }
        public async Task<bool> ChangeStatusItem(int id, bool status)
        {
            var dbObj = await _context.Country.
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
            var dbObj = await _context.Country.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbObj == null)
            {
                return false;
            }
            dbObj.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task SaveItem(Country obj)
        {
            if (obj.Id == 0)
            {
                obj.IsDeleted = false;
                obj.IsPublish = true;
                // obj.CreateDate = DateTime.Now;

                await _context.Country.AddAsync(obj);
            }
            else
            {

                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Tuple<IList<Country>, int>> LoadItemsData(string Search, int StatusId, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null, int languageId = 0)
        {
            var AllListCount = 0;
            var dataQuery = _context.Country
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
            AllListCount = await dataQuery.CountAsync();
            return new Tuple<IList<Country>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);

        }

        public int LoadItemsDataCategory(int? country)
        {
            var dataquery = _context.Category.Where(x => !x.IsDeleted);
            //dataquery=dataquery.Where(x=>x.c)

            var result = dataquery.AsQueryable();
            return result.Count();
        }
        public int LoadItemsDataCountry(int? country, int? SectorId)
        {
            if (SectorId == 0) {
                var dataquery = _context.Country.Where(x => !x.IsDeleted);
                if (country > 0)
                    dataquery = dataquery.Where(x => x.Id == country);
                var result = dataquery.AsQueryable();
                return result.Count();
            }
            else
            {
                var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                queryAlrowadData = queryAlrowadData.Where(x => x.SectorId == SectorId);
                var countries = queryAlrowadData.GroupBy(s => s.CountryId);
                return countries.Count();
            }
            
        }
        public int LoadItemsDataOrganization(int? CountryId, int? SectorId)
        {
            if (CountryId == 0)
            {
                if (SectorId > 0)
                {
                    var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                    queryAlrowadData = queryAlrowadData.Where(x => x.SectorId == SectorId);
                    var organizations = queryAlrowadData.GroupBy(s => s.OrganizationId);
                    return organizations.Count();
                }

                else {
                    var dataOrganization = _context.Organization.Where(x => !x.IsDeleted && x.IsPublish == true);
                    var result = dataOrganization.AsQueryable();
                    return result.Count();
                }
                
            }
            else
            {
                var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                queryAlrowadData = queryAlrowadData.Where(x => x.CountryId == CountryId);
                if (SectorId > 0)
                    queryAlrowadData = queryAlrowadData.Where(x => x.SectorId == SectorId);
                var organizations = queryAlrowadData.GroupBy(s => s.OrganizationId);
                return organizations.Count();
            }

        }
        public int LoadItemsDataSector(int? CountryId, int? SectorId)
        {
            if (CountryId == 0)
            {
                var querySector = _context.Sector.Where(x => !x.IsDeleted && x.IsPublish == true);
                if (SectorId > 0)
                    querySector = querySector.Where(x => x.Id == SectorId);
                var result = querySector.AsQueryable();
                return result.Count();
            }
            else
            {

                var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                    queryAlrowadData = queryAlrowadData.Where(x => x.CountryId == CountryId);
                if (SectorId > 0)
                    queryAlrowadData = queryAlrowadData.Where(x =>x.SectorId == SectorId);
                var sectors = queryAlrowadData.GroupBy(s => s.SectorId);
                return sectors.Count();
            }

        }
        public int LoadItemsDataOrganzationWithoutValue(int? CountryId, int? SectorId)
        {
            //  organization without values
            var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.ValueId == 56);
            if (CountryId > 0)
            {
                dataquery = dataquery.Where(x => x.CountryId == CountryId);
            }
            if (SectorId > 0)
            {
                dataquery = dataquery.Where(x => x.SectorId == SectorId);
            }
            var result = dataquery.AsQueryable();
            var distinctValues = result.
                     Select(x => x.OrganizationId).Distinct();
            return distinctValues.Count();
        }
        public int LoadItemsDataOrganzationWithValue(int? CountryId, int? SectorId)
        {
            // organization with values
            var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.ValueId != 56 && x.IsPublish == true);
            if (CountryId > 0) {
                dataquery = dataquery.Where(x=>x.CountryId == CountryId);
            }
            if (SectorId > 0) {
                dataquery = dataquery.Where(x=>x.SectorId == SectorId);
            }
            var result = dataquery.AsQueryable();
            var distinctValues = result.
                     Select(x => x.OrganizationId).Distinct();
            return distinctValues.Count();
        }
        public int LoadItemsDataValue(int? CountryId, int? SectorId)
        {
            if (CountryId == 0)
            {

                if (SectorId > 0)
                {
                    var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                    queryAlrowadData = queryAlrowadData.Where(x => x.SectorId == SectorId);
                    var organizations = queryAlrowadData.GroupBy(s => s.ValueId);
                    return organizations.Count();
                }

                else
                {
                    var dataValue = _context.Value.Where(x => !x.IsDeleted && x.IsPublish == true);
                    var result = dataValue.AsQueryable();
                    return result.Count();
                }

            }
            else
            {

                var queryAlrowadData = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                queryAlrowadData = queryAlrowadData.Where(x => x.CountryId == CountryId);
                if (SectorId > 0)
                    queryAlrowadData = queryAlrowadData.Where(x => x.SectorId == SectorId);
                var values = queryAlrowadData.GroupBy(s => s.ValueId) ;
                return values.Count();
            }
            
        }
    } //string sqlQuery = "select * from Organzation";
      //SqlCommand sql = new SqlCommand(sqlQuery);
}
