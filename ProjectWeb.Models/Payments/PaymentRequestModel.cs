using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Payments
{
    public class PaymentRequestModel
    {
        public string Content { get; set; }

        public Guid? RefID { get; set; }

        public decimal? RequiredAmount { get; set; }

        public Guid? MerchantID { get; set; }

        public Guid? PaymentDestinationID { get; set; }
        public string Language { get; set; }

        public string SignValue { get; set; }
    }

}
