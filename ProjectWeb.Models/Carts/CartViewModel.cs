using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Carts
{
    public class CartViewModel
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double PriceDollar { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }

    public class CartViewModelRequest
    {
        public Guid ID { get; set; }
    }
}
