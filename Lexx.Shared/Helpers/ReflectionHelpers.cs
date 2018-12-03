using System;
using System.Linq;
using System.Reflection;

namespace Lexx.Shared.Helpers
{
    public static class ReflectionHelper
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            var properties = propertyName.Split('.');

            var prop = GetProperty(obj, properties[0]);
            if (prop != null)
            {
                var value = prop.GetValue(obj);
                if (value != null && !IsSimple(value.GetType()))
                    return GetPropertyValue(value, properties.Length > 1 ? String.Join(".", properties.Skip(1)) : properties[0]);
                else
                    return value;
            }

            return null;
        }

        public static PropertyInfo GetProperty(object obj, string propertyName)
        {
            foreach (var prop in obj.GetType().GetProperties())
                if (prop.Name.ToLower() == propertyName.ToLower())
                    return prop;

            return null;
        }

        public static bool IsSimple(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type.Equals(typeof(string))
                || type.Equals(typeof(DateTime))
                || type.Equals(typeof(DateTimeOffset))
                || type.Equals(typeof(string[]))
                || type.Equals(typeof(decimal));
        }
    }
}