using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Common.Exceptions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Commons
{
    public class StorageServices : Repository<Image>, IStorageServices
    {
        private readonly ProjectWebDBContext _context;
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public StorageServices(ProjectWebDBContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }


        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task<Guid> AddImages(Guid entityId, ImageModel request)
        {
            var image = new Image()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductID = entityId,
            };
            if(request.ImageFile != null)
            {
                image.ImagePath = await this.SaveFile(request.ImageFile);
                image.FileSize = request.ImageFile.Length;
            }
            _context.Images.Add(image);
             await _context.SaveChangesAsync();
            return image.ID;
        }

        public async Task<int> UpdateImage(Guid imageId, ImageModel request)
        {
            var image = await _context.Images.FindAsync(imageId);

            if (image == null)
                throw new ProjectWebException("Cannot find image");

            if (request.ImageFile != null)
            {
                image.ImagePath = await this.SaveFile(request.ImageFile);
                image.FileSize = request.ImageFile.Length;
            }
            _context.Images.Update(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveImage(Guid imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if(image == null)
                throw new ProjectWebException("Cannot find image");

            _context.Images.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ImageModel>> GetListImageByProductID(Guid ProductID)
        {
            var listImages = await _context.Images.Where(x => x.ProductID == ProductID)
                .Select(x => new ImageModel()
            {
                ID = x.ID,
                Caption = x.Caption,
                FilePath = x.ImagePath,
                IsDefault = x.IsDefault.Value,
                FileSize = x.FileSize,
                ProductID = ProductID,
                Sort = x.Sort

            }).ToListAsync();

            return listImages;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
