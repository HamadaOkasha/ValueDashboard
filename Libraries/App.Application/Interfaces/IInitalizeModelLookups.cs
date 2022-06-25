using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using App.Application.ViewModels;

namespace App.Application.Interfaces
{

    public interface IInitalizeModelLookups
    {
        Task InitModel(AdminPagesModel model, string selectTitle = null);
        Task InitModel(GroupModel model, string selectTitle = null);
        Task InitModel(UserModel model, string selectTitle = null);
        Task InitModel(AlrowadDataModel model, string selectTitle = null);
        Task InitModel(HomeViewModel model, string selectTitle = null);
        //Task InitModel(OrderImageModel model, string selectTitle = null);
        // Task InitModel(OrderModel model, string selectTitle = null);
    }
}