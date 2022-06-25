using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Application.Model
{
    public partial class NotificationModel
    {

        public int Id { get; set; }
        public string MessageBody { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string FromCustomerId { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string ToCustomerId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsRead { get; set; }

        public SelectList CustomerList { get; set; }
    }
}
