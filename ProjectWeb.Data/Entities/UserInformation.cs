using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class UserInformation
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Sort { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }

        public SystemUser SysUser { get; set; }
    }
}
