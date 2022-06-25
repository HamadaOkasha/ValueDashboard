using App.Domain.Model;
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface ILookupRepository
    {
        Task<IList<AdminPages>> GetParentList();
        Task<IList<Country>> GetCountryList();
        Task<IList<Category>> GetCategoryList();
        Task<IList<Sector>> GetSectorList();
        Task<IList<Organization>> GetOrganizationList();
        Task<IList<Value>> GetValueList();
        Task<IList<ResearchValue>> GetResearchValueList();
        Task<IList<AlrowadVersion>> GetVersionList();


       //  Task<IList<Order>> GeOrderList();
       //Task<IList<OrderStatus>> GeStatusList();
    }
}