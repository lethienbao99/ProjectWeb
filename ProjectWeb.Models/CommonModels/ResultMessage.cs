using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class ResultMessage<T>
    {

        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T Object { get; set; }
    }
}
