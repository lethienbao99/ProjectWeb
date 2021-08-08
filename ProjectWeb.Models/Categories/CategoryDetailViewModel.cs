using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Home;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Categories
{
    public class CategoryDetailViewModel 
    {
        public string BaseURLApi { get; set; }
        public ResultMessage<CategoryViewModel> Category { get; set; }
        public ResultMessage<PageResultModel<ProductModel>> ItemProductsWithPaging { get; set; }
    }
}
