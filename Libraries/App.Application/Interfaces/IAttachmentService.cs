using Microsoft.AspNetCore.Http;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IAttachmentService
    {
        Task<Attachment> InsertPicture(long fileSize = 0, string fileName = "", string MimeType = "", string FileExtension = "");
        Task<Attachment> GetAttachmenById(int? Id);
        Task<string> GetAttachmenNameById(int? Id);
        Task<AttachmentModel> GetPictureById(int? Id);
        byte[] GetDownloadBits(IFormFile file);
        Task DeletePicture(int Id);

        //picture from Caching
        Task<string> GetPictureUrl(int? Id);
        Task<AttachmentsViewModel> GetPictureDataFromCache(int? Id);
        Task<string> GetPictureName(int? Id);
        Task<string> GetPictureUrlApi(int? Id);
    }
}
