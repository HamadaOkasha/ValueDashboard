using App.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class AlrowadData: BaseEntity, ILocalizedEntity
    {
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int SectorId { get; set; }
        public virtual Sector Sector { get; set; }
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public int ValueId { get; set; }
        public virtual Value Value { get; set; }
        public int ResearchValueId { get; set; }
        public virtual ResearchValue ResearchValue { get; set; }
        public int AlrowadVersionId { get; set; }
        public virtual AlrowadVersion AlrowadVersion { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }
    }
}
