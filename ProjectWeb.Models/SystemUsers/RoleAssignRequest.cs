using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.SystemUsers
{
    public class RoleAssignRequest
    {
        public Guid ID { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
