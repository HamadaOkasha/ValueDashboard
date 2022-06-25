using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class AttachmentsViewModel
    {
        public int? id { get; set; }
        public int tableId { get; set; }
        public bool deleted { get; set; }
        public string FileName { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public string thumbnailUrl { get; set; }
        public string ImagePath { get; set; }
        public string FileExtension { get; set; }

    }
}
