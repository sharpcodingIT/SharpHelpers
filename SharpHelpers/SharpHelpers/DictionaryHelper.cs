// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCoding.SharpHelpers
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// This method add the TKey with the specific format
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="formatString"></param>
        /// <param name="argList"></param>
        public static void AddFormat<TKey>(this Dictionary<TKey, string> dictionary,
            TKey key,
            string formatString,
            params object[] argList)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
                dictionary.Add(key, string.Format(formatString, argList));
        }

        /// <summary>
        /// This method remove items by the specific predicate
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="condition"></param>
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (condition == null) throw new ArgumentNullException(nameof(condition));

            foreach (var cur in dictionary.Where(condition).ToList())
            {
                dictionary.Remove(cur.Key);
            }
        }

        /// <summary>
        /// This method get or create the value related to the specific key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key) where TValue : new()
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            if (!dictionary.TryGetValue(key, out TValue ret))
            {
                ret = new TValue();
                dictionary[key] = ret;
            }
            return ret;
        }

        /// <summary>
        /// Tries to add a key-value pair to the dictionary. If the key already exists, it updates the value.
        /// </summary>
        /// <param name="dictionary">The dictionary to operate on.</param>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to associate with the key.</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Removes the entry with the specified key if it exists in the dictionary, and returns a boolean indicating success.
        /// </summary>
        /// <param name="dictionary">The dictionary to operate on.</param>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was found and removed, otherwise false.</returns>
        public static bool RemoveIfExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.Remove(key);
        }

        /// <summary>
        /// Merges the entries from another dictionary into the current dictionary. If a key already exists, its value is updated.
        /// </summary>
        /// <param name="dictionary">The dictionary to operate on.</param>
        /// <param name="otherDictionary">The dictionary to merge from.</param>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> otherDictionary)
        {
            foreach (var kvp in otherDictionary)
            {
                dictionary.AddOrUpdate(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Converts the dictionary into a readable string format, useful for debugging.
        /// </summary>
        /// <param name="dictionary">The dictionary to convert to a string.</param>
        /// <returns>A string representation of the dictionary.</returns>
        public static string ToReadableString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var entries = new List<string>();
            foreach (var kvp in dictionary)
            {
                entries.Add($"{kvp.Key}: {kvp.Value}");
            }
            return "{" + string.Join(", ", entries) + "}";
        }
    }
}
