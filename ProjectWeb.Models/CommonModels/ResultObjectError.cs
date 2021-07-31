using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class ResultObjectError<T> : ResultMessage<T>
    {
        public string[] ValdationMessages { get; set; }
        public ResultObjectError(string message)
        {
            IsSuccessed = false;
            Message = message;
        }

        public ResultObjectError(string[] messages)
        {
            IsSuccessed = false;
            ValdationMessages = messages;
        }
        public ResultObjectError()
        {
            IsSuccessed = false;
        }
    }
}
