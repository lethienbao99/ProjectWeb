using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.Entities
{
    public class AppConfig: BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
