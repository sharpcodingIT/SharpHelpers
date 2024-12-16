// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Text.Json;

namespace SharpCoding.SharpHelpers.ObjectExtensions
{
    public static class ObjectConverterHelper
    {
        #region Convert to int [Int32]
        /// <summary>
        /// Converts an object to an integer (Int32). Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="instance">The object to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted integer or the default value.</returns>
        public static int ToInt32(this object instance, int defaultValue = 0)
        {
            if (instance == null) return defaultValue;

            var valueTemp = instance.ToString();

            return valueTemp.ToInt32(defaultValue);
        }

        /// <summary>
        /// Converts an Enum to its integer (Int32) representation.
        /// </summary>
        /// <param name="instance">The Enum instance to convert.</param>
        /// <returns>The integer value of the Enum.</returns>
        public static int ToInt32(this Enum instance)
        {
            return Convert.ToInt32(instance);
        }

        /// <summary>
        /// Converts a string to an integer (Int32). Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="instance">The string to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted integer or the default value.</returns>
        public static int ToInt32(this string instance, int defaultValue = 0)
        {
            return int.TryParse(instance, out var result) ? result : defaultValue;
        }
        #endregion

        #region Convert to long [Int64]
        /// <summary>
        /// Converts an object to a long integer (Int64). Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="instance">The object to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted long integer or the default value.</returns>
        public static long ToInt64(this object instance, long defaultValue = 0)
        {
            if (instance == null) return defaultValue;

            var valueTemp = instance.ToString();

            return valueTemp.ToInt64(defaultValue);
        }

        /// <summary>
        /// Converts an Enum to its long integer (Int64) representation.
        /// </summary>
        /// <param name="instance">The Enum instance to convert.</param>
        /// <returns>The long integer value of the Enum.</returns>
        public static long ToInt64(this Enum instance)
        {
            return Convert.ToInt64(instance);
        }

        /// <summary>
        /// Converts a string to a long integer (Int64). Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="instance">The string to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted long integer or the default value.</returns>
        public static long ToInt64(this string instance, long defaultValue = 0)
        {
            return long.TryParse(instance, out var result) ? result : defaultValue;
        }
        #endregion

        /// <summary>
        /// Converts a string to a boolean value. Returns false if the conversion fails.
        /// </summary>
        /// <param name="instance">The string to convert.</param>
        /// <returns>The converted boolean value or false.</returns>
        public static bool ToBoolean(this string instance)
        {
            return bool.TryParse(instance, out var result) && result;
        }

        /// <summary>
        /// Converts a string to a DateTime. Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="instance">The string to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted DateTime or the default value.</returns>
        public static DateTime ToDateTime(this string instance, DateTime defaultValue = default)
        {
            return DateTime.TryParse(instance, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts a string to a specified Enum type. Returns a default value if the conversion fails.
        /// </summary>
        /// <typeparam name="T">The Enum type to convert to.</typeparam>
        /// <param name="instance">The string to convert.</param>
        /// <param name="defaultValue">The default value to return in case of failure.</param>
        /// <returns>The converted Enum value or the default value.</returns>
        public static T ToEnum<T>(this string instance, T defaultValue = default) where T : struct
        {
            return Enum.TryParse(instance, true, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// Converts a byte array to an object of a specified type. Returns null if the conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="instance">The byte array to convert.</param>
        /// <returns>The converted object or null.</returns>
        public static T ToObject<T>(this byte[] instance) where T : class
        {
            if (instance == null) return null;

            try
            {
                return JsonSerializer.Deserialize<T>(instance);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object to a byte array. Returns null if the serialization fails.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The byte array representing the object or null.</returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null) return null;

            try
            {
                return JsonSerializer.SerializeToUtf8Bytes(obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an integer to its string representation in a specified base (between 2 and 36).
        /// Returns an empty string if the base is invalid.
        /// </summary>
        /// <param name="number">The integer to convert.</param>
        /// <param name="targetBase">The base to convert to.</param>
        /// <returns>The string representation of the integer in the specified base.</returns>
        public static string ToBase(this int number, int targetBase)
        {
            if (targetBase < 2 || targetBase > 36) return string.Empty;
            if (targetBase == 10) return number.ToString();

            var result = string.Empty;
            var n = targetBase;
            var q = number;

            while (q > 0)
            {
                var r = q % n;
                q /= n;
                result = (r < 10 ? r.ToString() : ((char)(r + 55)).ToString()) + result;
            }

            return result;
        }
    }
}