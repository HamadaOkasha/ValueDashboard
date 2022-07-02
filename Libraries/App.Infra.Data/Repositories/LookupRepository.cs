
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
            return await _context.Country.Where(x => x.IsDeleted != true && x.IsPublish == true).OrderBy(x => x.Name).ToListAsync();

        }
        public async Task<IList<Category>> GetCategoryList()
        {
            return await _context.Category.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }

        public async Task<IList<Organization>> GetOrganizationList()
        {
            return await _context.Organization.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Value>> GetValueList()
        {
            return await _context.Value.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Sector>> GetSectorList(int CountryId = 0)
        {
            if (CountryId > 0)
            {
                var query = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);
                query = query.Where(x => x.CountryId == CountryId);
                var sectors = query.GroupBy(s => s.SectorId)
                           .Select(x => new
                           {
                               Id = x.Key,
                           //   NumberOfOrders = x.Count(),
                       });
                var dataQuery = from s in sectors
                                join g in _context.Sector on s.Id equals g.Id
                                orderby g.Name
                                select new Sector
                                {
                                    Name = g.Name,
                                    Id = s.Id
                                };
                return dataQuery.ToList();
            }
            else
                return await _context.Sector.Where(x => x.IsDeleted != true && x.IsPublish == true).OrderBy(x => x.Name).ToListAsync();

        }
        public async Task<IList<Category>> GetValueAndCountList(int CountryId = 0)
        {

            var query = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);

            if (CountryId > 0)
            {
                query = query.Where(x => x.CountryId == CountryId);
            }
            var topvalues = query.GroupBy(s => s.ValueId)
                        .Select(x => new
                        {
                            Value = x.Key,
                            NumberOfValues = x.Count(),

                        });

            var dataQuery = from c in topvalues
                            join g in _context.Value on c.Value equals g.Id orderby c.NumberOfValues descending, g.Name
                            select new Category
                            {
                                //Id = c.Value,
                                Name = g.Name,
                                Id = c.NumberOfValues
                            };

            return dataQuery.ToList();
            //return await _context.Value.Where(x => x.IsDeleted != true && x.IsPublish == true).ToListAsync();

        }
        public async Task<IList<Category>> GetCountryAndCountList(int Index = 0)
        {

            var query = _context.AlrowadData.Where(s => s.IsDeleted != true && s.IsPublish == true);

            var values = query.GroupBy(s => s.ValueId)
                        .Select(x => new
                        {
                            ValueId = x.Key,
                            NumberOfValues = x.Count(),

                        });
            var Countries = query.GroupBy(s => s.Country)
                        .Select(x => new
                        {
                            CountryId = x.Key,
                           // Name

                            //NumberOfValues = x.Count(),

                        });
            //var dataQuery1 = from v in values
            //                join g in _context.Value on v.ValueId equals g.Id
            //                select new TotalCount
            //                {
            //                    Id = v.ValueId,
            //                    Count = v.NumberOfValues
            //                };

            var dataQuery2 = from q in query
                             join c in _context.Country on q.CountryId equals c.Id
                            select new Category
                            {
                                Id = q.ValueId,
                                Name = c.ShortCode,
                              //  Count=c.value
                            };
            var dadataQuery3 = from q1 in values
                               join q2 in dataQuery2 on q1.ValueId equals q2.Id
                               select new Category
                               {
                                   Name = q2.Name,
                                   Id = q1.NumberOfValues,
                                  // Count = q1.NumberOfValues,
                               };
            //var dadataQuery3 = from q1 in dataQuery1
            //                   join q2 in dataQuery2 on q1.Id equals q2.Id
            //                   select new TotalCount
            //                   {
            //                       Name=q2.Name,
            //                       Count=q1.Count,
            //                   };

            return dadataQuery3.ToList();

        }

        //public class ChartvValues
        //{
        //    public int Id { get; set; }

        //    public string Name { get; set; }

        //    public int Count { get; set; }
        //}
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
