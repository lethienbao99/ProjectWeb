using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Cart: BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid UserID { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }
        public SystemUser SystemUser { get; set; }
    }
}
