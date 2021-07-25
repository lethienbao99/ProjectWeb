using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class BaseModel
    {
        public Guid ID { get; set; }
        public int Sort { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }
    }
}
