using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.SystemUsers
{
    public class TokenModel
    {
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }
    }
}
