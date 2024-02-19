using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Payments
{
    public class PaymentLinkResponseModel
    {
        public Guid? PaymentID { get; set; }
        public string PaymentUrl { get; set; }
    }
}
