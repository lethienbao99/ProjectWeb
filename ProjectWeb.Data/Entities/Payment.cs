using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    /// <summary>
    /// Thông tin 1 lần thanh thoán của hóa đơn
    /// </summary>
    public class Payment: BaseEntity
    {
        public string Content { get; set; }

        public string Currency { get; set; }

        public Guid? RefID { get; set; }

        public decimal RequiredAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string Language { get; set; }

        public decimal? PaidAmount { get; set; }

        public string Status { get; set; }

        public Guid? MerchantID { get; set; }

        public Guid? PaymentDestinationId { get; set; }
        public Guid? OrderID { get; set; }

        public virtual Merchant Merchant { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<PaymentSignature> PaymentSignatures { get; set; } = new List<PaymentSignature>();

    }
}
