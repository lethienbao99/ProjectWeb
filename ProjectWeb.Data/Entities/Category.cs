using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Category
    {
        public Guid ID { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Alias { get; set; }
        public int Sort { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

    }
}
