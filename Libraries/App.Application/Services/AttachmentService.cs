using Microsoft.AspNetCore.Http;
using App.Application.Caching;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;

namespace App.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        public readonly ICacheManager _cacheManager;
        public IAttachmentRepository _attachmentRepository;

        public  IConfiguration Configuration { get; set; }


        public AttachmentService(IConfiguration _configuration, IAttachmentRepository attachmentRepository, ICacheManager cacheManager)
        {
            _attachmentRepository = attachmentRepository;
            _cacheManager = cacheManager;
            Configuration = _configuration;

        }
        public byte[] GetDownloadBits(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return fileBytes;
                }
            }
        }
        public async Task<Attachment> GetAttachmenById(int? Id)
        {
            var result = await _attachmentRepository.GetItemByIdAsync(Id);

            return result;
        }
        public async Task DeletePicture(int Id)
        {
            await _attachmentRepository.DeleteItem(Id);
        }
        public async Task<string> GetAttachmenNameById(int? Id)
        {
            var result = await _attachmentRepository.GetAttachmentNameById(Id);

            return result;
        }
        public async Task<AttachmentModel> GetPictureById(int? Id)
        {
            if (Id == 0 || !Id.HasValue)
            {
                var item = new AttachmentModel();
                item.Attachment64 = "";
                item.FileName = "0.png";
                return item;
            }

            AttachmentModel model = new AttachmentModel();
            string url = "";
            string FileName = "";

            var result = await _attachmentRepository.GetItemByIdAsync(Id);
            if (result != null)
            {
                string fileName = string.Format("{0}_{1}", Id.Value, result.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AttachmentFiles", fileName);
                //  FileInfo MyFile = new FileInfo(path);

                model.DirectoryPath = path;
                model.FileExtension = Path.GetFullPath(path);
                model.FileSize = 0;

                //if (!File.Exists(path))
                //{
                //    //    url = "/AttachmentFiles/0.png";
                //    FileName = "0.png";
                //}
                //else
                //{
                //    //   url = "/AttachmentFiles/0.png";
                //    url = "/AttachmentFiles/" + fileName;
                //    FileName = fileName;
                //}
                url = "/AttachmentFiles/" + fileName;
                model.Attachment64 = url;
                model.AttachmentId = Id.Value;
                model.FileName = FileName;
            }
            else
            {
                return new AttachmentModel();
            }
            return model;
        }
        public async Task<Attachment> InsertPicture(long fileSize = 0, string fileName = "", string MimeType = "", string FileExtension = "")
        {
            Attachment obj = new Attachment()
            {
                // AttachmentContent = null,
                MimeTypeValue = MimeType,
                FileName = fileName,
                FileExtension = FileExtension,
                FileSize = fileSize,
            };

            await _attachmentRepository.InsertItem(obj);

            return obj;
        }
        public async Task<string> GetPictureUrl(int? Id)
        {
                var valuePicture = await GetPictureById(Id);
                if (valuePicture != null)
                {
                    return valuePicture.Attachment64;
                }
                return "";
        }
        public async Task<AttachmentsViewModel> GetPictureDataFromCache(int? Id)
        {
            var PictureCacheKey = string.Format(ModelCacheEventConsumer.PICTURE_DATA_BY_ID, Id);

            return await _cacheManager.GetAsync(PictureCacheKey, async () =>
            {
                var fileData = await GetPictureById(Id);
                var file = new AttachmentsViewModel();
                file.id = Id;
                file.FileName = fileData.FileName;
                file.FileExtension = fileData.FileExtension;
                file.size = fileData.FileSize;
                file.thumbnailUrl = fileData.Attachment64;
                file.name = fileData.FileName;
                file.deleted = false;

                return file;
            });
        }
        
     public async Task<string> GetPictureUrlApi(int? Id)
        {
            var PictureCacheKey = string.Format(ModelCacheEventConsumer.PICTURE_KEY_BY_ID, Id);

            return await _cacheManager.GetAsync(PictureCacheKey, async () =>
            {
                var valuePicture = await GetPictureById(Id);
                if (valuePicture != null)
                {
                    return !string.IsNullOrEmpty(valuePicture.Attachment64) ?
                    Configuration["AdminUrl"] + valuePicture.Attachment64
                    : "";
                }
                return "";
            });
        }
        public async Task<string> GetPictureName(int? Id)
        {
            var PictureCacheKey = string.Format(ModelCacheEventConsumer.PICTURE_NAME_KEY_BY_ID, Id);

            return await _cacheManager.GetAsync(PictureCacheKey, async () =>
            {
                var valuePicture = await GetPictureById(Id);
                if (valuePicture != null)
                {
                    return valuePicture.FileName;
                }
                return "";
            });
        }
    }
}
