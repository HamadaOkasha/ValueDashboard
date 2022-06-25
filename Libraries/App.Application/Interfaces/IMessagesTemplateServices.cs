using App.Application.Model;
using App.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IMessagesTemplateServices
    {
        Task<MessagesTemplateModel> GetMessagesTemplateById(int id);
        Task<Tuple<IList<MessagesTemplateViewModel>, int>> LoadMessagesTemplates(int TypeId, string Search,
             int jtStartIndex = 0, int jtPageSize = 10, string order = null,
            string orderDir = null);

        Task SaveMessagesTemplate(MessagesTemplateModel obj);
        Task<bool> ChangeStatus(int id, bool status);
        MessagesTemplateModel GetMessageBody(int id, string Name = null, int orderNo = 0);
        Task<MessagesTemplateModel> PopulateForgetEmail(string Name = null, string Url = null);
        Task<MessagesTemplateModel> GetById(int id);
        Task Save(MessagesTemplateModel model);
        Task<Tuple<IList<MessagesTemplateViewModel>,int>> LoadData( string Search = null,
           int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);
    }
}