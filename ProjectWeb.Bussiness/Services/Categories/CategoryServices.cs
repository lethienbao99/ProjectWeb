using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
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

    }
}
