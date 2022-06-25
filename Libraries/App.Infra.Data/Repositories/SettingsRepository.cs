
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using App.Domain.Interfaces;
using App.Domain.Model;
using App.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace App.Infra.Data.Repositories
{

    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;
        public SettingsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MailSetting> LoadMailSetting()
        {
            var result = await _context.MailSetting
                .FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }
        public async Task<bool> SaveMailSetting(MailSetting obj)
        {
            if (obj.Id == 0)
                await _context.MailSetting.AddAsync(obj);
            else
                _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
