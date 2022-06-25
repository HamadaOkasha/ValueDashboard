using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class MessagesTemplateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationDateText
        {
            get
            {
                return CreationDate.HasValue ? CreationDate.Value.ToString("yyyy/MM/dd") : null;
            }
        }
    }
}
