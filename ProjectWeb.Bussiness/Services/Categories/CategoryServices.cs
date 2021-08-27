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

        public async Task<ResultMessage<int>> Create(CategoryCreateOrUpdateRequest request)
        {
            if(request.IsParent == true)
            {
                var category = new Category()
                {
                    ID = Guid.NewGuid(),
                    CategoryName = request.CategoryName,
                    Code = request.Code,
                    Description = request.Description,
                    Alias = request.Alias,
                    DateCreated = DateTime.Now
                };
                await _context.Categories.AddAsync(category);
                var result = await _context.SaveChangesAsync();
                return new ResultObjectSuccess<int>(result);
            }
            else if(request.ParentID != null && request.IsParent == false)
            {
                var category = new Category()
                {
                    ID = Guid.NewGuid(),
                    CategoryName = request.CategoryName,
                    Code = request.Code,
                    Description = request.Description,
                    Alias = request.Alias,
                    ParentID = request.ParentID,
                    DateCreated = DateTime.Now
                };
                await _context.Categories.AddAsync(category);
                var result = await _context.SaveChangesAsync();
                return new ResultObjectSuccess<int>(result);
            }

            return new ResultObjectError<int>();
        }

        public async Task<ResultMessage<int>> Update(CategoryCreateOrUpdateRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == request.ID);
            if(category != null)
            {
                if(request.IsParent == true && request.ParentID != null)
                {
                    category.CategoryName = request.CategoryName;
                    category.Code = request.Code;
                    category.Description = request.Description;
                    category.Alias = request.Alias;
                    category.DateUpdated = request.DateUpdated;
                    category.ParentID = request.ParentID;
                }
                else
                {
                    category.CategoryName = request.CategoryName;
                    category.Code = request.Code;
                    category.Description = request.Description;
                    category.Alias = request.Alias;
                    category.DateUpdated = request.DateUpdated;
                }
                _context.Categories.Update(category);
                var result = await _context.SaveChangesAsync();
                return new ResultObjectSuccess<int>(result);
            }
            return new ResultObjectSuccess<int>();
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

        public async Task<ResultMessage<PageResultModel<CategoryViewModel>>> GetAllPaging(CategoryPagingRequest request)
        {
            var query = from c in _context.Categories
                        select c;

            //Filter.
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.CategoryName.Contains(request.Keyword) || x.Description.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).OrderByDescending(x => x.Sort)
                 .Select(x => new CategoryViewModel()
                 {
                     ID = x.ID,
                     CategoryName = x.CategoryName,
                     Code = x.Code,
                     Description = x.Code,
                     Type = x.Type
                 }).ToListAsync();

            var pagedResult = new PageResultModel<CategoryViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return new ResultObjectSuccess<PageResultModel<CategoryViewModel>>(pagedResult);
        }

  
    }
}
