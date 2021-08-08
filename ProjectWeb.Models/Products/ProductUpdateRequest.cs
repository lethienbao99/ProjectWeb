using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Products
{
    public class ProductUpdateRequest
    {
        public Guid ID { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public double PriceDollar { get; set; }
        public int Stock { get; set; }
        public string Alias { get; set; }
        public int Views { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
