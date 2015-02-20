using System.Reflection;

namespace System
{
    public static class ExtensionMethods
    {
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
