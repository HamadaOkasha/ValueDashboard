
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Infra.Data.Repositories
{

    public class NotificationRepository : INotificationRepository
    {
        public AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tuple<IList<Notification>, int>> LoadItemsData( int AllListCount, int jtStartIndex = 0,
         int jtPageSize = 10, string CurrentCustomerId = null, bool? IsRead = null, string Search = null, string order = null, string orderDir = null)
        {
            AllListCount = 0;

            var dataQuery = _context.Notification
                .Where(s =>  s.IsRead == IsRead || !IsRead.HasValue);
            if (!string.IsNullOrEmpty(CurrentCustomerId))
            {
                dataQuery = dataQuery.Where(x => x.ToCustomerId == CurrentCustomerId);
            }


            if (!string.IsNullOrEmpty(Search))
            {
                Search = Regex.Replace(Search.ToLower(), @"\s+", "");
                dataQuery = dataQuery.Where(g => g.MessageBody.ToLower().Contains(Search.ToLower())
                || g.FromCustomer.FullName.Contains(Search));
            }
            var result = dataQuery.Include(s => s.FromCustomer).AsQueryable();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "0":
                        result = orderDir == "asc" ? result.OrderBy(x => x.Id) : result.OrderByDescending(x => x.Id);
                        break;
                    case "1":
                        result = orderDir == "asc" ? result.OrderBy(x => x.MessageBody) : result.OrderByDescending(x => x.MessageBody);
                        break;
                    case "2":
                        result = orderDir == "asc" ? result.OrderBy(x => x.CreateDate) : result.OrderByDescending(x => x.CreateDate);
                        break;
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.Id);
            }
            if (result.Count() > 0)
                AllListCount = result.Count();

            if (jtPageSize != 8)
            {
                if (dataQuery.Where(s => s.IsRead != true).Any())
                {
                    dataQuery.Where(s => s.IsRead != true).ToList().ForEach(g => { g.IsRead = true; });
                    _context.SaveChanges();
                }
                AllListCount = await dataQuery.CountAsync();
                return new Tuple<IList<Notification>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
            }
            else
            {
                AllListCount = await dataQuery.CountAsync();
                return new Tuple<IList<Notification>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
            }
        }



        public async Task<Tuple<IList<Notification>, int>> LoadItemsDataApi(int AllListCount, int jtStartIndex = 0,
        int jtPageSize = 10, string CurrentCustomerId = null)
        {
            AllListCount = 0;

            var dataQuery = _context.Notification
                .Where(x => x.ToCustomerId == CurrentCustomerId);          

           var result = dataQuery.Include(s => s.FromCustomer).AsQueryable();
           
             result = result.OrderByDescending(x => x.Id);
          
            if (result.Count() > 0)
                AllListCount = result.Count();

            if (jtPageSize != 8)
            {
                if (dataQuery.Where(s => s.IsRead != true).Any())
                {
                    dataQuery.Where(s => s.IsRead != true).ToList().ForEach(g => { g.IsRead = true; });
                    _context.SaveChanges();
                }
                AllListCount = await dataQuery.CountAsync();
                return new Tuple<IList<Notification>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
            }
            else
            {
                AllListCount = await dataQuery.CountAsync();
                return new Tuple<IList<Notification>, int>(await result.Skip(jtStartIndex).Take(jtPageSize).ToListAsync(), AllListCount);
            }
        }

        public void SendNotifications(Notification obj, string currentCustomerId)
        {
            _context.Notification.Add(obj);
            _context.SaveChanges();
        }
    }
}
