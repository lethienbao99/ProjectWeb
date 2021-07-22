using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Order
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipNumberPhone { get; set; }
        public string Status { get; set; }
        public int Sort { get; set; }
        public DateTime? DateOrderd { get; set; }
        public DateTime? DateRejected { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
