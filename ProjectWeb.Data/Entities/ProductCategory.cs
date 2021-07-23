using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class ProductCategory: BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid CategoryID { get; set; }


        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
