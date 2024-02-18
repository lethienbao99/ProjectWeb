using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    /// <summary>
    /// Bảng chứa thông tin dịch vụ thanh toán: Momo, VNPay
    /// </summary>
    public class Merchant: BaseEntity
    {
        public string MerchantName { get; set; }

        public string MerchantPayLink { get; set; }

        public string MerchantIpnUrl { get; set; }

        public string MerchantReturnUrl { get; set; }

        public string SerectKey { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
