using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class ProductCategory
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public Guid CategoryID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }


        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
