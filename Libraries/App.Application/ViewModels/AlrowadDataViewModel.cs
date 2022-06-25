using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class AlrowadDataViewModel
    {
        public int Id { get; set; }
        public string CountryId { get; set; }
        public string CategoryId { get; set; }
        public string SectorId { get; set; }
        public string OrganizationId { get; set; }
        public string ValueId { get; set; }
        public string ResearchValueId { get; set; }
        public string AlrowadVersionId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }

    }
}
