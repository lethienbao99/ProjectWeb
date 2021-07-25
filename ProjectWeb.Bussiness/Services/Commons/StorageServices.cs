using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Task<int> AddImages(int imageId, List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            throw new NotImplementedException();
        }

        public Task<List<ImageModel>> GetListImageByProductID(Guid ProductID)
        {
            throw new NotImplementedException();
        }
    }
}
