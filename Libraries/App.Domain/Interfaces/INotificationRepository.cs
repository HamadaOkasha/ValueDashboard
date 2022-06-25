using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public  interface INotificationRepository
    {
        Task<Tuple<IList<Notification>, int>> LoadItemsData(int AllListCount, int jtStartIndex = 0,
         int jtPageSize = 10, string CurrentCustomerId = null, bool? IsRead = null, string Search = null, string order = null, string orderDir = null);
      void  SendNotifications(Notification obj, string currentCustomerId);

        Task<Tuple<IList<Notification>, int>> LoadItemsDataApi(int AllListCount, int jtStartIndex = 0,
        int jtPageSize = 10, string CurrentCustomerId = null);


    }
}
