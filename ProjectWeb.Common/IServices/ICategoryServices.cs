using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface ICategoryServices : IRepository<Category>
    {
        Task<ResultMessage<List<CategoryViewModel>>> GetAllByCreateOrUpdate();
    }
}
