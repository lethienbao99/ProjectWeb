using ProjectWeb.Models;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.IServiceBackendAPIs
{
    public interface ICategoryBackendAPI
    {
        Task<ResultMessage<List<CategoryViewModel>>> GetAll();
        Task<ResultMessage<List<CategoryViewModel>>> GetAllByCreateOrUpdate(bool isParent);
        Task<ResultMessage<CategoryViewModel>> GetCategoryByID(Guid ID);
        Task<ResultMessage<PageResultModel<CategoryViewModel>>> GetAllPaging(CategoryPagingRequest request);
        Task<ResultMessage<int>> Create(CategoryCreateOrUpdateRequest request);
        Task<ResultMessage<int>> Update(Guid ID, CategoryCreateOrUpdateRequest request);
    }
}
