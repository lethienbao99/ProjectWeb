using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Order: BaseEntity
    {
        public Guid UserID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipNumberPhone { get; set; }
        public string Status { get; set; }
        public DateTime? DateOrderd { get; set; }
        public DateTime? DateRejected { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }
        public SystemUser SystemUser { get; set; }

    }
}
