using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Messages
{
    public class MessageViewModel
    {
        public string TitleText { get; set; }
        public string MessageText { get; set; }
        public string Guest { get; set; }
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public Guid ProductID { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
