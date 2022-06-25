using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infra.Data.Repositories
{

    public class AttachmentRepository : IAttachmentRepository
    {
        public AppDbContext _context;
        public AttachmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task InsertItem(Attachment obj)
        {
            await _context.Attachments.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Attachment> GetItemByIdAsync(int? Id)
        {
            var result = await _context.Attachments.Where(p => p.AttachmentId == Id)
                                                   .FirstOrDefaultAsync();

            return result;
        }
        public Attachment GetItemById(int? Id)
        {
            var result = _context.Attachments.Where(p => p.AttachmentId == Id)
                .FirstOrDefault();

            return result;
        }
        public async Task<string> GetAttachmentNameById(int? Id)
        {
            var result = await _context.Attachments.Where(p => p.AttachmentId == Id)
                .Select(s => s.FileName).FirstOrDefaultAsync().ConfigureAwait(false);

            return result;
        }
        public async Task DeleteItem(int Id)
        {
            var item = await _context.Attachments.Where(p =>  p.AttachmentId == Id)
              .FirstOrDefaultAsync().ConfigureAwait(false);
            if (item != null)
            {
                _context.Attachments.Remove(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}
