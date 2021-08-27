using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Categories
{
    public class CategoryCreateOrUpdateRequest
    {
        public Guid ID { get; set; }
        public Guid? ParentID { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string Sort { get; set; }
        public bool IsParent { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
