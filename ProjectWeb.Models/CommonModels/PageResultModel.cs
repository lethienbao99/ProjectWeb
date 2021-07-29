using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class PageResultModel<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord { get; set; }
    }

    public class PagingRequestBase: RequestBaseModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
