using App.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class MessageTemplate : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int? TypeId { get; set; }
    }
}
