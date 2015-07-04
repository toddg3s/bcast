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

        public static bool IsValidAccount(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return false;
            if (value.Length > 128) return false;
            for (int i = 0; i < value.Length; i++)
                if (!char.IsLetterOrDigit(value[i]))
                    return false;
            return true;
        }

        public static bool IsValidName(this string value)
        {
            // For now, names and accounts have the same rules.  If this changes, put the implementation here
            return IsValidAccount(value);
        }

        public static string EndpointName(this string account, string name)
        {
            if (!account.IsValidAccount())
                return null;
            if (!name.IsValidName())
                return null;
            return account + "." + name;
        }
    }
}
