using System.Reflection;
using System.Text.RegularExpressions;

namespace System
{
    public static class ExtensionMethods
    {
        public const string IgnoreDuringCompare = "~SKIP~";

        /// <summary>
        /// Compares if two string are with equal values. If ignore string (ExtensionMethods.IgnoreDuringCompare) is present it is skipped during compare
        /// </summary>
        public static bool EqualsWithIgnore(this string value1, string value2)
        {
            string regexPattern = "(.*?)";
            // If value is null set it to empty
            value1 = value1 ?? string.Empty;
            value2 = value2 ?? string.Empty;
            string input = string.Empty;
            string pattern = string.Empty;
            // Unify new lines symbols
            value1 = value1.Replace("\r\n", "\n");
            value2 = value2.Replace("\r\n", "\n");
            // If no one conains ignore string then compare directly
            if (!value1.Contains(IgnoreDuringCompare) && !value2.Contains(IgnoreDuringCompare))
            {
                return value1.Equals(value2);
            }
            else if (value1.Contains(IgnoreDuringCompare))
            {
                pattern = Regex.Escape(value1).Replace(IgnoreDuringCompare, regexPattern);
                input = value2;
            }
            else if (value2.Contains(IgnoreDuringCompare))
            {
                pattern = Regex.Escape(value2).Replace(IgnoreDuringCompare, regexPattern);
                input = value1;
            }

            Match match = Regex.Match(input, pattern);
            return match.Success;
        }

        /// <summary>
        /// Get string with HHmmss of current DateTime
        /// </summary>
        public static string TimeStamp(this DateTime date)
        {
            return date.ToString("HHmmss");
        }

        /// <summary>
        /// Get value of StingValue Enum attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            string stringValue = "No StringValue assigned for '" + value.ToString() + "' enum member";
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValue[] attrs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
            {
                stringValue = attrs[0].Value;
            }
            return stringValue;
        }
    }
}
