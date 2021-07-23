using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class OrderDetail: BaseEntity
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public int Quatity { get; set; }
        public double TotalPrice { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
