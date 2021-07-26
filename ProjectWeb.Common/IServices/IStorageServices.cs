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

        Task<Guid> AddImages(Guid entityId, ImageModel request);
        Task<int> UpdateImage(Guid imageId, ImageModel request);
        Task<int> RemoveImage(Guid imageId);
        Task<List<ImageModel>> GetListImageByProductID(Guid ProductID);
    }
}
