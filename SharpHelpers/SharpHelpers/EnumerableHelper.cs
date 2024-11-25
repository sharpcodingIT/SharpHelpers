// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCoding.SharpHelpers
{
    public static class EnumerableHelper
    {
        /// <summary>
        /// This method returns a sublist of the instance after a distinctby operation
        /// </summary>
        /// <param name="list"></param>
        /// <param name="propertySelector"></param>
        /// <exception cref="System.ArgumentNullException">throw when <paramref name="list"/> is null</exception>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> enumerable, Func<T, object> propertySelector)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            return enumerable
                 .GroupBy(propertySelector)
                 .Select(x => x.First());
        }

        /// <summary>
        /// This method returns a sublist of the instance after a distinctby
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            return enumerable
                .GroupBy(x => x)
                .Select(x => x.First());
        }

        /// <summary>
        /// This method returns a list with duplicates counters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> enumerable, Func<T, object> propertySelector)
        {
                if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
                if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

                var enumer = enumerable
                     .GroupBy(propertySelector)
                     .Where(k=>k.Count()>1)
                     .Select(g =>(T)g.First());
          
            return enumer;
             
        }

        /// <summary>
        /// This method returns the duplicates object count on the specific property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static int CountDuplicates<T>(this IEnumerable<T> enumerable, Func<T, object> propertySelector)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            return enumerable
                .GroupBy(propertySelector).Where(k => k.Count() > 1)
                .Select(g => g.Count()).Sum();
               
        }

        /// <summary>
        /// Sets value at speciefied index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrSet<T>(this IEnumerable<T> enumerable, int index, T value)
        {   
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            
            if ((enumerable as List<T>).Count < index) return false;
            if ((enumerable as List<T>).Count > index)
            {
                (enumerable as List<T>)[index] = value;
                return true;
            }
            (enumerable as List<T>).Add(value);
            
            return true;
        }

        /// <summary>
        /// This method checks if all the element of the list are serializable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsSerializable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            return enumerable.All(item => item.GetType().IsSerializable);
        }

        /// <summary>
        /// This method returns a string of items with delimiter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> enumerable, string delimiter)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            return string.Join(delimiter, enumerable.ToArray());
        }

        /// <summary>
        /// This method splits the list into 'size' sublists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> enumerable, int size)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (size < 0) throw new ArgumentException(nameof(size));
            var list = enumerable.ToList();
            var splitList = new List<List<T>>();
            for (var i = 0; i < list.Count; i += size)
            {
                splitList.Add(list.GetRange(i, Math.Min(size, list.Count - i)));
            }
            return splitList;
        }

        /// <summary>
        /// Check if the list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Apply an action to each element of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Chunk the list into sublists of the specified size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "La dimensione deve essere maggiore di zero.");

            return source.Select((x, i) => new { Index = i, Value = x })
                         .GroupBy(x => x.Index / size)
                         .Select(g => g.Select(x => x.Value));
        }

        /// <summary>
        /// Get a random element from the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var list = source.ToList();

            if (list.Count == 0)
                throw new InvalidOperationException("La sequenza non contiene elementi.");

            Random rng = new Random();
            int index = rng.Next(list.Count);
            return list[index];
        }

        /// <summary>
        /// Check if all elements in the list are distinct.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AllDistinct<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var seen = new HashSet<T>();
            foreach (var item in source)
            {
                if (!seen.Add(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Random rng = new Random();
            return source.OrderBy(_ => rng.Next());
        }

        /// <summary>
        /// Sum the elements of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int Sum<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Returns the maximum element based on a given selector function.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Aggregate((maxItem, nextItem) => selector(nextItem).CompareTo(selector(maxItem)) > 0 ? nextItem : maxItem);
        }

        /// <summary>
        /// Returns the minimum element based on a given selector function.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable<TKey>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Aggregate((minItem, nextItem) => selector(nextItem).CompareTo(selector(minItem)) < 0 ? nextItem : minItem);
        }

        /// <summary>
        /// Finds the index of the first element that satisfies a given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            int index = 0;
            foreach (var item in source)
            {
                if (predicate(item)) return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Checks if the source contains any of the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var set = new HashSet<T>(items);
            return source.Any(set.Contains);
        }

        /// <summary>
        /// Checks if the source contains all of the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var set = new HashSet<T>(source);
            return items.All(set.Contains);
        }

        /// <summary>
        /// Returns the median of a sequence of numbers.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double Median(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var sortedList = source.OrderBy(n => n).ToList();
            int count = sortedList.Count;
            if (count == 0)
                throw new InvalidOperationException("The source sequence is empty.");

            if (count % 2 == 0)
            {
                return (sortedList[count / 2 - 1] + sortedList[count / 2]) / 2.0;
            }
            else
            {
                return sortedList[count / 2];
            }
        }
    }
}
