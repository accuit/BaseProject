using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// class for extension methods for IQueryable
    /// </summary>
    public static class IQueryableExtension
    {
        /// <summary>
        /// Extension method for Skip take query over the data collection using LINQ expressions
        /// </summary>
        /// <typeparam name="T">Generic type of collection</typeparam>
        /// <param name="query">queriable collection</param>
        /// <param name="skipIndex">skip index value</param>
        /// <param name="takeIndex">take index value</param>
        /// <returns>returns collection</returns>
        public static IQueryable<T> SkipTakeQuery<T>(this IQueryable<T> query, int skipIndex, int takeIndex)
        {
            if (skipIndex == -1)
            {
                return query;
            }
            return query.Skip(skipIndex).Take(takeIndex);
        }

        /// <summary>
        /// Extension method to validate whether collection is null or have zero records
        /// </summary>
        /// <param name="records">data collection</param>
        /// <returns>returns Boolean value</returns>
        public static bool IsNullOrZero(this ICollection records)
        {
            if (records == null || records.Count == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Extension method to validate whether collection is null or have zero records
        /// </summary>
        /// <param name="records">data collection</param>
        /// <returns>returns Boolean value</returns>
        public static bool IsNullOrZero<T>(this ICollection<T> records)
        {
            if (records == null || records.Count == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Extension method to perform action for each item available in collection
        /// </summary>
        /// <typeparam name="T">Generic type of collection</typeparam>
        /// <param name="source">source object</param>
        /// <param name="action">action to execute</param>
        /// <returns>returns updated collection</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
                action(item);

            return source;
        }

        /// <summary>
        /// Extension method to append target collection to source collection 
        /// </summary>
        /// <typeparam name="T">Generic type of collection</typeparam>
        /// <param name="source">source collection</param>
        /// <param name="target">target collection</param>
        public static void AddRange<T>(this ICollection<T> sourceList, IEnumerable<T> targetList)
        {
            foreach (var target in targetList)
            {
                sourceList.Add(target);
            }
        }
    }
}
