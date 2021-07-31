using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class ResultObjectSuccess<T> : ResultMessage<T>
    {
        public ResultObjectSuccess()
        {
            IsSuccessed = true;
        }
        public ResultObjectSuccess(T resultObject)
        {
            IsSuccessed = true;
            Message = "Success";
            Object = resultObject;
        }
    }
}
