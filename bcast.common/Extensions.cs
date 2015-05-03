using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public static class Extensions
    {
        public static string Truncate(this string value, int maxlength, bool elipsis = true)
        {
            if (value == null) return null;

            if (value.Length <= maxlength) return value;

            var val = value.Substring(0, (elipsis ? maxlength - 3 : maxlength));
            return val + ((elipsis) ? "..." : "");
        }
    }
}
