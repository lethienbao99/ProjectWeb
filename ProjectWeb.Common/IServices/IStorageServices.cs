using Microsoft.AspNetCore.Http;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IStorageServices : IRepository<Image>
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);

        Task<int> AddImages(int imageId, List<IFormFile> files);
        Task<int> UpdateImage(int imageId, string caption, bool isDefault);
        Task<List<ImageModel>> GetListImageByProductID(Guid ProductID);
    }
}
