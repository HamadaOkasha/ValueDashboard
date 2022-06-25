using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class AttachmentModel
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public string DirectoryPath { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; set; }
        public string Attachment64 { get; set; }
        public byte[] AttachmentContent { get; set; }
        public byte[] AttachmentContent202 { get; set; }
        public byte[] AttachmentContent57 { get; set; }
    }
}
