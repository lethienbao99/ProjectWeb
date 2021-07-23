using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class SystemUser : IdentityUser<Guid>
    {
        public int Sort { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }

        public Guid UserInfomationID { get; set; }
        public UserInformation UserInfomation { get; set; }
        public List<Order> Orders { get; set; }
        public List<Cart> Carts { get; set; }

    }
}
