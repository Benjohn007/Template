using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Core
{
    public static class PermissionManager
    {
        public static class User
        {
            const string ROLE_NAME = "user";

            public const string VIEW = $"{ROLE_NAME}.view";
            public const string UPDATE = $"{ROLE_NAME}.update";
            public const string ADD = $"{ROLE_NAME}.update";
        }

        public static class Role
        {
            const string ROLE_NAME = "role";

            public const string VIEW = $"{ROLE_NAME}.view";
            public const string UPDATE = $"{ROLE_NAME}.update";
            public const string ADD = $"{ROLE_NAME}.update";
        }
    }

}
