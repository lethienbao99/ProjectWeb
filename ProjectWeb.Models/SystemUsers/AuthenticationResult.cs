using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.SystemUsers
{
    public class AuthenticationResult
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string ErrorString { get; set; }
    }
}
