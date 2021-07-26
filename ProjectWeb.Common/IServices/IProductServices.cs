using Microsoft.AspNetCore.Http;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IProductServices : IRepository<Product>
    {
        Task<Guid> CreateWithImages(ProductModel request);
        Task<int> UpdateWithImages(ProductModel request);
        Task<PageResultModel<ProductViewModel>> GetAllPaging(ProductPagingRequest request);
        Task<PageResultModel<ProductModel>> GetAllByCategoryId(ProductByCategoryIdPagingRequest request);
        Task<ProductViewModel> GetProductByID(Guid ID);
    }
}
