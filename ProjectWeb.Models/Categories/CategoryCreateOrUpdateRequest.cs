using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Categories
{
    public class CategoryCreateOrUpdateRequest
    {
        public Guid ID { get; set; }
        [DisplayName("Thuộc loại sản phẩm cha")]
        public Guid? ParentID { get; set; }
        [DisplayName("Tên loại sản phẩm")]
        public string CategoryName { get; set; }
        [DisplayName("Mã loại sản phẩm")]
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        public string Alias { get; set; }
        public string Sort { get; set; }
        [DisplayName("Là loại sản phẩm \"cha\"")]
        public bool IsParent { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
