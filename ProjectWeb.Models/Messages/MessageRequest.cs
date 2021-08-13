using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Messages
{
    public class MessageRequest
    {
        public string TitleText { get; set; }
        public string MessageText { get; set; }
        public string Guest { get; set; }
        public Guid UserID { get; set; }
        public Guid ProductID { get; set; }
    }
}
