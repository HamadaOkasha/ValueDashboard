
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

    public class MessagesTemplateRepository : IMessagesTemplateRepository
    {
        private readonly AppDbContext _context;
        public MessagesTemplateRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MessageTemplate> GetMessagesTemplateById(int id)
        {
            var result = await _context.MessageTemplate.Where(s=> s.Id == id).FirstOrDefaultAsync();

            return result;
        }
        public async Task<bool> SaveMessagesTemplate(MessageTemplate obj)
        {
            if (obj.Id == 0)
               await _context.MessageTemplate.AddAsync(obj);
            else
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

           await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ChangeStatus(int id, bool status)
        {
            var obj = await GetMessagesTemplateById(id);
            obj.IsActive = status;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Tuple<IList<MessageTemplate>,int>> LoadMessagesTemplates(int TypeId, string Search,
        int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null)
        {
            var dataQuery = _context.MessageTemplate.
                Where(x => x.TypeId == TypeId);
            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = dataQuery
                    .Where(x => x.Title.Contains(Search));
            }
            var result = dataQuery.AsQueryable();

            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Id) : result.OrderByDescending(x => x.Id);
                        break;
                    case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Title) : result.OrderByDescending(x => x.Title);
                        break;
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Description) : result.OrderByDescending(x => x.Description);
                        break;
                }
            }
            else
            {
                result = result.OrderBy(x => x.Id);
            }
            int AllListCount = await dataQuery.CountAsync();

            return new Tuple<IList<MessageTemplate>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(),AllListCount);
        }
        public async Task<MessageTemplate> GetItemById(int Id)
        {
            return await _context.MessageTemplate
                .Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task SaveItem (MessageTemplate obj)
        {
            if (obj.Id == 0)
            {

                await _context.MessageTemplate.AddAsync(obj);
            }
            else
            {
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Tuple<IList<MessageTemplate>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null)
        {
            
            var dataQuery = _context.MessageTemplate.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                dataQuery = dataQuery
                     .Where(x => x.Title.Contains(Search));
            }

            var result = dataQuery.AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Title) : result.OrderByDescending(x => x.Title);
                        break;
                    case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.TypeId) : result.OrderByDescending(x => x.TypeId);
                        break;
                }
            }
            else
            {
                result = result.
                    OrderByDescending(x => x.Id);
            }
            int AllListCount = await dataQuery.CountAsync();

            return new Tuple<IList<MessageTemplate>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
        }
    }
}
