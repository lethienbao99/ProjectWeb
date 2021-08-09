using Microsoft.EntityFrameworkCore;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Categories
{
    public class CategoryServices : Repository<Category>, ICategoryServices
    {
        private readonly ProjectWebDBContext _context;
        public CategoryServices(ProjectWebDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultMessage<List<CategoryViewModel>>> GetAllByCreateOrUpdate()
        {
            var categories = await _context.Categories.Where(x => x.ParentID != null && x.ParentID != Guid.Empty).Select(x => new CategoryViewModel()
            {
                ID = x.ID,
                CategoryName = x.CategoryName + " - " + x.Code
            }).ToListAsync();

            if (categories != null)
                return new ResultObjectSuccess<List<CategoryViewModel>>(categories);
            return new ResultObjectError<List<CategoryViewModel>>("List category is null");
            
        }
    }
}
