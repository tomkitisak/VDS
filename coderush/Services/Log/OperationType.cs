using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vds.Services.Log
{
    public static class OperationType
    {
        public static string CREATE => "CREATE";

        public static string READ => "READ";

        public static string UPDATE => "UPDATE";

        public static string DELETE => "DELETE";
    }
}
