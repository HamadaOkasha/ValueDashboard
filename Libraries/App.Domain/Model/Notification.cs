using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Model
{
    public partial class Notification
    {

        
            public int Id { get; set; }
            public string MessageBody { get; set; }
            public string ToCustomerId { get; set; }
            public string FromCustomerId { get; set; }
            public Nullable<System.DateTime> CreateDate { get; set; }
            public Nullable<bool> IsRead { get; set; }
            [ForeignKey("ToCustomerId")]
            public virtual AspNetUsers ToCustomer { get; set; }
            [ForeignKey("FromCustomerId")]
            public virtual AspNetUsers FromCustomer { get; set; }

        
    }
}
