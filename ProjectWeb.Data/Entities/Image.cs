using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class Image : BaseEntity
    {
        public Guid ProductID { get; set; }
        public string ImagePath { get; set; }
        public long FileSize { get; set; }
        public string Caption { get; set; }
        public bool? IsDefault { get; set; }

        public Product Product { get; set; }
    }
}
