using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Token : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public SystemUser User { get; set; }
    }
}
