using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Products
{
    public class ProductCategoriesServices : Repository<ProductCategory>, IProductCategoriesServices
    {
        private readonly ProjectWebDBContext _context;
        private readonly IStorageServices _storageServices;

        public ProductCategoriesServices(ProjectWebDBContext context, IStorageServices storageServices) : base(context)
        {
            _context = context;
            _storageServices = storageServices;
        }
    }
}
