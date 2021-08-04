using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.IServiceBackendAPIs
{
    public interface IProductBackendAPI
    {
        Task<ResultMessage<PageResultModel<ProductModel>>> GetProductPaging(ProductPagingRequest request);
        Task<ResultMessage<bool>> Create(ProductCreateRequest request);
        Task<ResultMessage<bool>> Update(Guid ID, ProductCreateRequest request);
        Task<ResultMessage<ProductModel>> GetProductByID(Guid ID);
        Task<ResultMessage<bool>> Delete(Guid ID);
    }
}
