
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
    public class GeneralSettingRepository : IGeneralSettingRepository
    {
        public AppDbContext _context;
        public GeneralSettingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<GeneralSetting> GetItem(int Id)
        {
            return await _context.GeneralSetting.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<GeneralSetting> GetCurrentItem()
        {
            return await _context.GeneralSetting.FirstOrDefaultAsync();
        }
        public async Task SaveItem(GeneralSetting obj)
        {
            if (obj.Id == 0)
            {
                await _context.GeneralSetting.AddAsync(obj);
            }
            else
            {

                _context.Entry(obj).State =  Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
           await _context.SaveChangesAsync();
        }
        
    }
}
