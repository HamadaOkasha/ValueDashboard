using Microsoft.AspNetCore.Http;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface INotificationService
    {
        Task<Tuple<IList<NotificationViewModel>, int>> LoadNotifcation(int AllListCount, int jtStartIndex = 0, int jtPageSize = 10, string CurrentCustomerId = null, bool? IsRead = null, string Search = null, string order = null, string orderDir = null);
        void SendNotifications(NotificationModel model, string currentCustomerId);
        Task<Tuple<IList<NotificationViewModel>, int>> LoadNotifcationapi(int jtStartIndex = 0, int jtPageSize = 10, string CurrentCustomerId = null);
    }
}
