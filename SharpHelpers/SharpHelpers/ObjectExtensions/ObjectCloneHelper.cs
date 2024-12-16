// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SharpCoding.SharpHelpers.ObjectExtensions
{
    public static class ObjectCloneHelper
    {
        #region Private Properties

        private const BindingFlags Binding = BindingFlags.Instance |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Public |
                                             BindingFlags.FlattenHierarchy;

        #endregion

        /// <summary>
        /// Clones an object and returns a deep copy of type T.
        /// Excluded properties can be specified via a list of property names.
        /// </summary>
        /// <typeparam name="T">The type of the cloned object.</typeparam>
        /// <param name="instance">The object to clone.</param>
        /// <param name="propertyExcludeList">A list of property names to exclude from cloning.</param>
        /// <returns>A deep copy of the object, or default(T) if the object is null.</returns>
        public static T Clone<T>(this object instance, ICollection<string> propertyExcludeList = null)
        {
            if (instance == null)
                return default;

            return (T)DeepClone(instance, propertyExcludeList);
        }

        /// <summary>
        /// Clones an object and returns a deep copy.
        /// </summary>
        /// <param name="instance">The object to clone.</param>
        /// <returns>A deep copy of the object.</returns>
        public static object Clone(this object instance)
        {
            return DeepClone(instance);
        }

        #region Private Method: DeepClone

        /// <summary>
        /// Recursively clones an object and its children.
        /// </summary>
        /// <param name="instance">The object to clone.</param>
        /// <param name="propertyExcludeList">A list of property names to exclude from cloning.</param>
        /// <returns>A deep copy of the object.</returns>
        private static object DeepClone(object instance, ICollection<string> propertyExcludeList = null)
        {
            if (instance == null)
                return null;

            var primaryType = instance.GetType();

            // Handle arrays
            if (primaryType.IsArray)
                return ((Array)instance).Clone();

            // Handle collections (IList)
            if (typeof(IList).IsAssignableFrom(primaryType))
            {
                var listType = typeof(List<>).MakeGenericType(primaryType.GetGenericArguments().FirstOrDefault() ?? typeof(object));
                var listClone = (IList)Activator.CreateInstance(listType);

                foreach (var item in (IList)instance)
                {
                    listClone.Add(item == null ? null : DeepClone(item, propertyExcludeList));
                }

                return listClone;
            }

            // Handle strings
            if (primaryType == typeof(string))
                return string.Copy((string)instance);

            // Handle value types (primitives, structs, enums)
            if (primaryType.IsValueType || primaryType.IsPrimitive || primaryType.IsEnum)
                return instance;

            // Handle complex objects
            var clonedObject = FormatterServices.GetUninitializedObject(primaryType);
            var fields = primaryType.GetFields(Binding);

            foreach (var field in fields)
            {
                // Skip excluded fields
                if (propertyExcludeList != null && propertyExcludeList.Any())
                {
                    var fieldName = field.Name.ExtractBetween("<", ">")?.FirstOrDefault() ?? field.Name;
                    if (propertyExcludeList.Contains(fieldName))
                        continue;
                }

                // Skip readonly fields
                if (field.IsInitOnly)
                    continue;

                var value = field.GetValue(instance);

                // Clone child objects if they are classes (except strings)
                var clonedValue = field.FieldType.IsClass && field.FieldType != typeof(string)
                    ? DeepClone(value, propertyExcludeList)
                    : value;

                field.SetValue(clonedObject, clonedValue);
            }

            return clonedObject;
        }

        #endregion
    }

    /// <summary>
    /// Helper extensions for string operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extracts a substring between two delimiters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="startDelimiter">The starting delimiter.</param>
        /// <param name="endDelimiter">The ending delimiter.</param>
        /// <returns>An enumerable of substrings found between the delimiters.</returns>
        public static IEnumerable<string> ExtractBetween(this string input, string startDelimiter, string endDelimiter)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(startDelimiter) || string.IsNullOrEmpty(endDelimiter))
                return Enumerable.Empty<string>();

            var results = new List<string>();
            var startIndex = 0;

            while ((startIndex = input.IndexOf(startDelimiter, startIndex)) != -1)
            {
                startIndex += startDelimiter.Length;
                var endIndex = input.IndexOf(endDelimiter, startIndex);

                if (endIndex == -1)
                    break;

                results.Add(input[startIndex..endIndex]);
                startIndex = endIndex + endDelimiter.Length;
            }

            return results;
        }
    }
}