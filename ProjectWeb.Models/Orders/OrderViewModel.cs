using ProjectWeb.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Orders
{
    public class OrderViewModel
    {
        public Guid UserID { get; set; }
        public string ShipEmail { get; set; }
        public string ShipAddress { get; set; }
        public List<CartViewModel> ListItems { get; set; }
    }
}
