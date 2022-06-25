using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using App.Domain.Model;

namespace App.Infra.Data.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMessagesTemplateRepository _messagesTemplateRepository;
        public UserRepository(
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context, 
            IEmailSender emailSender, 
            IMessagesTemplateRepository messagesTemplateRepository)
        {
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailSender = emailSender;
            _messagesTemplateRepository = messagesTemplateRepository;
        }
        public async Task<AspNetUsers> GetUserById(string userId)
        {
            var result = await _context.AspNetUsers.Where(s=> s.Id == userId).Include(s=> s.AspNetUserRoles).FirstOrDefaultAsync();
            return result;
        }
        public async Task UpdateCustomerToken(string id, string token)
        {
            var user = await _context.AspNetUsers.Where(s => s.Id == id).FirstOrDefaultAsync();
            user.MobileToken = token;
            await _context.SaveChangesAsync();
        }
      
        public async Task<AspNetUsers> GetUserByEmail(string email)
        {
            var result = await _context.AspNetUsers.Where(s => s.UserName == email)
                .Include(s=> s.AspNetUserRoles)
                .ThenInclude(s=> s.Role)
                .FirstOrDefaultAsync();
            return result;
        }
        public async Task<string> GetAllFullNameUsersByIds(string[] userIds)
        {
            var result = await _context.AspNetUsers.Where(s => userIds.Contains(s.Id))
                .Select(s=> s.FullName).ToListAsync();
            
            return string.Join(",", result);
        }
        public async Task<IList<AdminPages>> GeAdminPagesList(string[] roleNames)
        {
            var query = _context.AdminPages.Include(s => s.RolePages);

            if (roleNames.Contains("Administrator"))
            {
                return query.OrderBy(s => s.DisplayOrder).ToList();
            }
            else
            {
                var roleIds = _context.AspNetRoles.Where(s => roleNames.Contains(s.Name)).Select(g=> g.Id);

                var listPages =await query.Include(s => s.RolePages)
                    .Where(s => s.RolePages.Where(g => roleIds.Contains(g.RoleId)).Any())
                    .ToListAsync();

                var newList = new List<AdminPages>();
                newList.AddRange(listPages);
                foreach (var item in listPages.Where(s=> s.ParentId > 0))
                {
                    var parent = _context.AdminPages.Where(s => s.Id == item.ParentId).FirstOrDefault();
                    if (!newList.Where(s=> s.Id == parent.Id).Any())
                        newList.Add(parent);
                }
                return newList.OrderBy(s => s.DisplayOrder).ToList();
            }
        }
        public async Task<Tuple<IList<AspNetUsers>, int>> LoadItemsData(string email,  string fullName,   string phoneNumber, string roleName, int StatusId, int jtStartIndex = 0,
         int jtPageSize = 10, string order = null, string orderDir = null)
        {

            var dataQuery = _context.AspNetUsers
                .Where(x => (StatusId == 0 || (StatusId == 1 && x.LockoutEnabled == false)
                || (StatusId == 2 && x.LockoutEnabled == true)));

       
            if (!string.IsNullOrEmpty(fullName))
            {
                dataQuery = dataQuery
                    .Where(x => x.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                dataQuery = dataQuery
                    .Where(x => x.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                dataQuery = dataQuery
                    .Where(x => x.PhoneNumber.Contains(phoneNumber));
            }

            dataQuery = dataQuery.Include(s => s.AspNetUserRoles)
               .Where(x => !x.AspNetUserRoles.Where(s => s.RoleId == "1" ).Any());

            if (!string.IsNullOrEmpty(roleName))
            {
                var roleId = _roleManager.FindByNameAsync(roleName).Result.Id;

                dataQuery = dataQuery
                 .Where(x => x.AspNetUserRoles.Where(s => s.RoleId == roleId).Any());
            }
            if (string.IsNullOrEmpty(roleName))
            {
                dataQuery = dataQuery.Include(s => s.AspNetUserRoles)
                 .Where(x => !x.AspNetUserRoles.Where(s => s.RoleId == "2").Any());
            }

            var result = dataQuery.AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.FullName) : result.OrderByDescending(x => x.FullName);
                        break;
                    case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.UserName) : result.OrderByDescending(x => x.UserName);
                        break;
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Email) : result.OrderByDescending(x => x.Email);
                        break;
                    case "3":
                        result = orderDir == "asc" ? result.OrderBy(x => x.PhoneNumber) : result.OrderByDescending(x => x.PhoneNumber);
                        break;
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            }
            int AllListCount = await dataQuery.CountAsync();

            return new Tuple<IList<AspNetUsers>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(),AllListCount);
        }
       
       
        public async Task<IList<AspNetUsers>> LoadChartCustomer(string roleName,DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var dataQuery = _context.AspNetUsers.Where(x => x.CreateDate >= StartDate &x.CreateDate <= EndDate).AsQueryable();
            if (!string.IsNullOrEmpty(roleName))
            {
                var roleId = _roleManager.FindByNameAsync(roleName).Result.Id;

                dataQuery = dataQuery
                 .Where(x => x.AspNetUserRoles.Where(s => s.RoleId == roleId).Any());
            }
            var list = dataQuery.GroupBy(p => new { p.CreateDate.Value.Date, p.Id })
                  .Select(f => new
                  {
                      CreateDate = f.Key.Date,
                      Id = f.Key.Id,
                      Count = f.Count(),
                  });

            var finalList = from c in _context.AspNetUsers
                            join meta in list on c.Id equals meta.Id
                            select new AspNetUsers
                            {
                                Id = c.Id,
                                UnitNumber = meta.Count,
                                CreateDate = meta.CreateDate,
                            };

            return await finalList.ToArrayAsync();
        }
    }
}
