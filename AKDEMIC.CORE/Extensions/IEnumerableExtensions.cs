using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.CORE.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> item)
        {
            return !item.Except(enumerable).Any();
        }

        public static int FindIndex<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            int index = 0;

            foreach (var arg in enumerable)
            {
                if (predicate(arg))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.FindIndex(x => EqualityComparer<T>.Default.Equals(item, x));
        }

        public static IEnumerable<T> OrderByCondition<T, TResult>(this IEnumerable<T> enumerable, bool condition, Func<T, TResult> keySelector)
        {
            if (condition)
            {
                return enumerable.OrderByDescending(keySelector);
            }
            else
            {
                return enumerable.OrderBy(keySelector);
            }
        }

        public static IEnumerable<T> OrderByDescendingCondition<T, TResult>(this IEnumerable<T> enumerable, bool condition, Func<T, TResult> keySelector)
        {
            if (condition)
            {
                return enumerable.OrderByDescending(keySelector);
            }
            else
            {
                return enumerable.OrderBy(keySelector);
            }
        }
    }
}
