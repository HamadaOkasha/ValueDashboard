using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IAttachmentRepository
    {
        Task InsertItem(Attachment obj);
        Task<Attachment> GetItemByIdAsync(int? Id);
        Attachment GetItemById(int? Id);
        Task<string> GetAttachmentNameById(int? Id);
        Task DeleteItem(int Id);
    }
}
