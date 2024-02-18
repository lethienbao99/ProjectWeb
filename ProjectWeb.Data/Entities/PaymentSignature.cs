using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class PaymentSignature : BaseEntity
    {
        public string SignValue { get; set; }

        public string SignAlgo { get; set; }

        public DateTime? SignDate { get; set; }

        public string SignOwn { get; set; }

        public Guid? PaymentID { get; set; }
        public bool? IsValid { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
