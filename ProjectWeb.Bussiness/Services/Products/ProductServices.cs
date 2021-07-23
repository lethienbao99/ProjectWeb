using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Products
{
    public class ProductServices : Repository<Product>, IProductServices
    {
        public ProductServices(ProjectWebDBContext context) : base(context)
        {

        }
        
    }
}
