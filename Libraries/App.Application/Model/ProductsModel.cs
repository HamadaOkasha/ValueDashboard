using App.Application.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class ProductsModel : BaseModel, ILocalizedModel<ProductsLocalizedModel>
    {
        public ProductsModel()
        {
            Locales = new List<ProductsLocalizedModel>();
            ListImages = new List<AttachmentsViewModel>();
        }
        public int? ImageId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public string[] SwitchCategoryId { get; set; }
        public string[] ProductImageId { get; set; }
        public IList<AttachmentsViewModel> ListImages { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublish { get; set; }
        public SelectList CategoryList { get; set; }
        public IList<ProductsLocalizedModel> Locales { get; set; }
    }
    public partial class ProductsLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
    }
}
