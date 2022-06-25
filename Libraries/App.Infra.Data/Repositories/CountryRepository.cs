
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
        public int LoadItemsDataCountry(int? country)
        {
            var dataquery = _context.Country.Where(x => !x.IsDeleted /*&& x.Id == country*/);
            // var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.CountryId== _context.Country.);
            var result = dataquery.AsQueryable();
            return result.Count();
        }
        public int LoadItemsDataOrganization()
        {
            //string sqlQuery = "select * from Organzation";
            //SqlCommand sql = new SqlCommand(sqlQuery);
            var dataquery = _context.Organization.Where(x => !x.IsDeleted);

            var result = dataquery.AsQueryable();
            return result.Count();
        }
        public int LoadItemsDataSector()
        {
            var dataquery = _context.Sector.Where(x => !x.IsDeleted);
            var result = dataquery.AsQueryable();
            return result.Count();
        }
        public int LoadItemsDataOrganzationWithoutValue()
        {
            //  organization without values
            var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.ValueId == 56);
            var result = dataquery.AsQueryable();
            var distinctValues = result.
                     Select(x => x.OrganizationId).Distinct();
            return distinctValues.Count();
            //var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.ValueId == null);
            //var result = dataquery.AsQueryable();
            //return result.Count();
        }
        public int LoadItemsDataOrganzationWithValue()
        {
            // organization with values
            var dataquery = _context.AlrowadData.Where(x => !x.IsDeleted && x.ValueId != 56);
            var result = dataquery.AsQueryable();
            var distinctValues = result.
                     Select(x => x.OrganizationId).Distinct();
            return distinctValues.Count();

        }
        public int LoadItemsDataValue()
        {
            // organization with values
            var dataquery = _context.Value.Where(x => !x.IsDeleted);
            var result = dataquery.AsQueryable();
            return result.Count();

        }
        //public IList<TopTenViewModel> LoadItemsDataValueAndName()
        //{
        //    var dataquery = _context.Value.Where(x => !x.IsDeleted);
        //    var result = dataquery.AsQueryable();
        //    //return result.Count();
        //}
        //public void LoadItemsDataValueAndName()
        //{
        //    //var result = _context.AlrowadData.Join(_context.Value, c => c.ValueId, v => v.Id,
        //    //   new TopTenViewModel {
        //    //       ValueName = v.Name
        //    //       ValueCount =
        //    //   }).tolist() ;
        //    //var query =
        //    //from ad in AlrowadData
        //    //join v in Value on ad.ValueID equals v.Id
        //    //select new
        //    //{
        //    //    Name = v.name,
        //    //    PetName = count ad.ValueId
        //    //};
        //    //    var dataquery = _context.AlrowadData.Join(AlrowadData, Value,AlrowadData.ValueId, Value.ID).
        //    //SELECT COUNT(DISTINCT arw_data.organization_id) AS "Frequancy",
        //    //    arw_values.title FROM arw_data INNER JOIN arw_values 
        //    //    ON arw_data.value_id = arw_values.id group by
        //    //    arw_data.value_id order by Frequancy DESC

        //     var dataquery = _context.Value.Where(x => !x.IsDeleted);

        //    var result = dataquery.AsQueryable();
        //    //return result.Count();
        //}
    }
}
