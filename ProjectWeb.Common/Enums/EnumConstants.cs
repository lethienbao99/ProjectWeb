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
            public const string Token = "Token";
            public const string SettingLanguage = "Language";
            public const string BaseURLApi = "BaseURLApi";
            public const string CartSession = "CartSession";
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
