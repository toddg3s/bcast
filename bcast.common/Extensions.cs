using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static bool IsValidPassword(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return false;
            if (value.Length > 32 || value.Length < 8) return false;

            int uppers = 0, lowers = 0, numbers = 0, others = 0;
            foreach(var ch in value)
            {
                if (Char.IsUpper(ch))
                {
                    uppers++;
                }
                else if (Char.IsLower(ch))
                {
                    lowers++;
                }
                else if (Char.IsNumber(ch))
                {
                    numbers++;
                }
                else
                {
                    others++;
                }
                if(uppers > 0 && lowers > 0 && numbers > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidEmail(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return false;

            return Regex.IsMatch(value, @"[\w-]+@([\w-]+\.)+[\w-]+");
        }

        public static string EndpointName(this string account, string name)
        {
            if (!account.IsValidAccount())
                return null;
            if (!name.IsValidName())
                return null;
            return account + "." + name;
        }

        public static EndpointType ToEndpointType(this string value)
        {
            if(String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is null or empty");
            }
            try
            {
                return (EndpointType)Enum.Parse(typeof(EndpointType), (string)value);
            }
            catch(ArgumentException)
            {
                var strvalue = value.ToString().ToLower();
                if (String.IsNullOrWhiteSpace(strvalue))
                    throw new ArgumentException();
                switch(strvalue[0])
                {
                    case 'w':
                        if (strvalue.StartsWith("wind"))
                            return EndpointType.Windows;
                        if (strvalue.StartsWith("winp"))
                            return EndpointType.WinPhone;
                        break;
                    case 'm':
                        return EndpointType.Mac;
                    case 'l':
                        return EndpointType.Linux;
                    case 'i':
                        return EndpointType.IOS;
                    case 'a':
                        return EndpointType.Android;
                    case 'o':
                        return EndpointType.Other;
                    default:
                        throw new ArgumentException();
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Error while getting EndpointType", ex);
            }
            throw new ArgumentException("Unknown");
        }
    }
}
