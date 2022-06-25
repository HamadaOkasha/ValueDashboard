using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Model
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string MimeTypeValue { get; set; }
        public string FileExtension { get; set; }
    }
}
