using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.Enums
{
    public class EnumConstants
    {
        public class SystemsConstants
        {
            public const string ConnectionString = "ProjectWebDB";
        }
        public class PublicConstants
        {

        }
        public enum ProductStatus
        {
            InActive,
            Active
        }
    }
}
