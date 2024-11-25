// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Linq;

namespace SharpCoding.SharpHelpers
{
    public static class ObjectHelper
    {
        private static readonly Type[] SystemTypes =
        {
            typeof(Enum),
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        /// <summary>
        /// Checks if the given type is a system type (e.g., primitive, value types, common system types).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a system type; otherwise, false.</returns>
        public static bool IsSystemType(this Type type)
        {
            if (type.IsValueType || type.IsPrimitive)
                return true;

            // Handle Nullable types
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsSystemType(type.GetGenericArguments()[0]);

            // Check against predefined system types
            if (SystemTypes.Contains(type))
                return true;

            // Check the type code to cover other system types
            return Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}
