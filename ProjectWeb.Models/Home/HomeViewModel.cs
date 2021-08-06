using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Home
{
    public class HomeViewModel
    {
        public List<ProductModel> ItemProducts { get; set; } = new List<ProductModel>();
    }
}
