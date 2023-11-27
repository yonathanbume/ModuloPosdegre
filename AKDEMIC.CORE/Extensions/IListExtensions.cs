using System;
using System.Collections.Generic;

namespace AKDEMIC.CORE.Extensions
{
    public static class IListExtensions
    {
        public static T BinarySearch<T, TResult>(this IList<T> enumerable, Func<T, TResult> keySelector, TResult other)
            where TResult : IComparable<TResult>
        {
            var enumerableCount = enumerable.Count;

            if (enumerableCount == 0)
            {
                return default(T);
            }

            var minIndex = 0;
            var maxIndex = enumerableCount - 1;

            while (minIndex < maxIndex)
            {
                var midIndex = minIndex + ((maxIndex - minIndex) / 2);
                var midItem = enumerable[midIndex];
                TResult midKey = keySelector(midItem);
                var comparison = midKey.CompareTo(other);

                if (comparison < 0)
                {
                    minIndex = midIndex + 1;
                }
                else if (comparison > 0)
                {
                    maxIndex = midIndex - 1;
                }
                else
                {
                    return midItem;
                }
            }

            var minItem = enumerable[minIndex];

            if (minIndex == maxIndex && minIndex < enumerableCount && keySelector(minItem).CompareTo(other) == 0)
            {
                return minItem;
            }

            return default(T);
        }

        /*public static T BinarySearch<T>(this IList<T> enumerable, Func<T, bool> keySelector)
        {
            var enumerableCount = enumerable.Count;

            if (enumerableCount == 0)
            {
                throw new InvalidOperationException();
            }

            var minIndex = 0;
            var maxIndex = enumerableCount;

            while (minIndex < maxIndex)
            {
                var midIndex = minIndex + ((maxIndex - minIndex) / 2);
                var midItem = enumerable[midIndex];
                var midKey = keySelector(midItem);

                if (midKey)
                {
                    minIndex = midIndex + 1;
                }
                else if (minIndex == midIndex)
                {
                    return midItem;
                }
                else
                {
                    maxIndex = midIndex;
                }
            }

            var minItem = enumerable[minIndex];

            if (minIndex == maxIndex && minIndex < enumerableCount)
            {
                return minItem;
            }

            return default(T);
        }*/
    }
}
