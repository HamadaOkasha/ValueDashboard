using App.Domain.Interfaces;
using App.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Application.Model
{
    public class AlrowadDataModel : BaseModel//, ILocalizedModel<AlrowadDataLocalizedModel>
    {
        //public AlrowadDataModel()
        //{
        //   // Locales = new List<AlrowadDataLocalizedModel>();
        //}
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int CountryId { get; set; }
        public SelectList CountryList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int CategoryId { get; set; }
        public SelectList CategoryList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int SectorId { get; set; }
        public SelectList SectorList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int OrganizationId { get; set; }
        public SelectList OrganizationList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int ValueId { get; set; }
        public SelectList ValueList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int ResearchValueId { get; set; }
        public SelectList ResearchValueList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FieldRequired")]
        public int AlrowadVersionId { get; set; }
        public SelectList VersionList { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsDeleted { get; set; }

      //  public IList<AlrowadDataLocalizedModel> Locales { get; set; }
    }
    //public partial class AlrowadDataLocalizedModel : ILocalizedLocaleModel
    //{
    //    public int LanguageId { get; set; }
    //    public string Name { get; set; }

    //}
    //public partial class AlrowadData: BaseEntity, ILocalizedEntity
    //{
    //    public int CountryId { get; set; }
    //    public virtual Country Country { get; set; }
    //    public int CategoryId { get; set; }
    //    public virtual Category Category { get; set; }
    //    public int SectorId { get; set; }
    //    public virtual Sector Sector { get; set; }
    //    public int OrganizationId { get; set; }
    //    public virtual Organization Organization { get; set; }
    //    public int ValueId { get; set; }
    //    public virtual Value Value { get; set; }
    //    public int ResearchValueId { get; set; }
    //    public virtual ResearchValue ResearchValue { get; set; }
    //    public int VersionId { get; set; }
    //    public virtual Version Version { get; set; }
    //    public string CreateBy { get; set; }
    //    public DateTime CreateDate { get; set; }
    //    public string UpdateBy { get; set; }
    //    public DateTime UpdateDate { get; set; }
    //    public bool IsPublish { get; set; }
    //    public bool IsDeleted { get; set; }
    //}
}
