using App.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Domain.Model
{
    public partial class TotalCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
