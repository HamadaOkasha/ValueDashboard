using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IMessagesTemplateRepository
    {
        Task<MessageTemplate> GetMessagesTemplateById(int id);
        Task<Tuple<IList<MessageTemplate>,int>> LoadMessagesTemplates(int TypeId, string Search,
            int jtStartIndex = 0, int jtPageSize = 10, string order = null, string orderDir = null);
        Task<bool> SaveMessagesTemplate(MessageTemplate obj);
        Task<bool> ChangeStatus(int id, bool status);
        Task<MessageTemplate> GetItemById(int id);
        Task<Tuple<IList<MessageTemplate>, int>> LoadItemsData(string Search, int jtStartIndex = 0,
            int jtPageSize = 10, string order = null, string orderDir = null);
        Task SaveItem(MessageTemplate obj);
    }
}
