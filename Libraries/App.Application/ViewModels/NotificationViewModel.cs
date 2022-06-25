using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.ViewModels
{
    public partial class NotificationViewModel
    {
        public string MessageBody { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerProfilePicture { get; set; }
        public string MyName { get; set; }
        public int TotelIsRead { get; set; }
        public int TotelIsReadNotifcatio { get; set; }
        public string MyProfilePicture { get; set; }
        public string NotificationDate { get; set; }
        public bool IsMe { get; set; }
        public int MessageId { get; set; }
        public int TotalRecored { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Since
        {
            get
            {
                var dateOrder = CreateDate.Value;

                return dateOrder.ToString("dd/MM/yyy hh:mm tt");
                
            }
        }
    }
}
