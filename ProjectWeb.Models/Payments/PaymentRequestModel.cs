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

        public string Currency { get; set; }

        public Guid? RefID { get; set; }

        public decimal? RequiredAmount { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpireDate { get; set; } = DateTime.UtcNow.AddMinutes(15);

        public string Language { get; set; }

        public Guid? MerchantID { get; set; }

        public Guid? PaymentDestinationID { get; set; }
        public string SignValue { get; set; }
    }

}
