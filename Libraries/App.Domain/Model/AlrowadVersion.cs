using App.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class AlrowadVersion: BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }
    }
}
