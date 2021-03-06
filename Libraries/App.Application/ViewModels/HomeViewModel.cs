using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.ViewModels
{
    public class HomeViewModel
    {
        public int TotalCategory { get; set; }
        public int TotalCountry { get; set; }
        public int TotalOrganization { get; set; }
        public int TotalSector { get; set; }
        public int TotalValue { get; set; }
        public int TotalOrganizationWithValue { get; set; }
        public int TotalOrganizationWithoutValue { get; set; }
        public int CountryId { get; set; }
        public SelectList CountryList { get; set; }
        public int SectorId { get; set; }
        public SelectList SectorList { get; set; }
        public SelectList TopTenValueAndCountList { get; set; }
        public SelectList CountryAndCountList { get; set; }
    }
}
