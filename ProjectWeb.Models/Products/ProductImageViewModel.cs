using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Products
{
    public class ProductImageViewModel
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int Sort { get; set; }
        public long? FileSize { get; set; }
    }
}
