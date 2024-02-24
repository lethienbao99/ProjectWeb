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
        public string StatusPayment { get; set; }
        public double TotalPrice { get; set; }
        public DateTime? DateOrderd { get; set; }
        public DateTime? DateRejected { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual SystemUser SystemUser { get; set; }
        public virtual Payment Payment { get; set; }

    }
}
