using AKDEMIC.CORE.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class IQueryableExtensions
    {
        public static void LookupProperties<T>(this IQueryable<T> queryable, Action<PropertyInfo, string> action, int depth = 3, string category = null, List<string> memberNames = null, List<Type> baseTypes = null)
        {
            var queryableElementType = queryable.ElementType;
            var typeProperties = queryableElementType.GetProperties();

            for (var i = 0; i < typeProperties.Length; i++)
            {
                typeProperties[i].Lookup(action, depth, category, memberNames);
            }
        }

        public static IQueryable<T> OrderByConditionThenBy<T, TResult>(this IQueryable<T> queryable, string condition, Expression<Func<T, TResult>> keySelector, Expression<Func<T, TResult>> ThenBykeySelector)
        {
            if (keySelector == null) return queryable.AsQueryable();

            if (condition == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            {
                if (ThenBykeySelector == null) 
                    return queryable.OrderByDescending(keySelector).AsQueryable();

                return queryable
                    .OrderByDescending(keySelector)
                    .ThenByDescending(ThenBykeySelector)
                    .AsQueryable();
            }
            else
            {
                if (ThenBykeySelector == null)
                    return queryable.OrderBy(keySelector).AsQueryable();

                return queryable
                    .OrderBy(keySelector)
                    .ThenBy(ThenBykeySelector)
                    .AsQueryable();
            }
        }
        public static IQueryable<T> OrderByCondition<T, TResult>(this IQueryable<T> queryable, string condition, Expression<Func<T, TResult>> keySelector)
        {
            if (keySelector == null) return queryable.AsQueryable();

            if (condition == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
            {
                return queryable
                    .OrderByDescending(keySelector)
                    .AsQueryable();
            }
            else
            {
                return queryable
                    .OrderBy(keySelector)
                    .AsQueryable();
            }
        }

        public static IQueryable<T> OrderByDescendingCondition<T, TResult>(this IQueryable<T> queryable, bool condition, Expression<Func<T, TResult>> keySelector)
        {
            if (condition)
            {
                return queryable
                    .OrderByDescending(keySelector)
                    .AsQueryable();
            }
            else
            {
                return queryable
                    .OrderBy(keySelector)
                    .AsQueryable();
            }
        }

        public static IQueryable<TResult> Select<TSource, TResult, TItem>(this IQueryable<TSource> queryable, Expression<Func<TSource, TResult>> selector, string searchValue, Expression<Func<TSource, TItem>> searchFilter = null)
        {
            var queryableSelect = queryable.Select(selector);
            searchValue = searchValue?.TrimSpaces();
            searchValue = searchValue?.ToUpper();

            if (searchValue == null || searchValue == "")
            {
                return queryableSelect;
            }

            var typeByte = typeof(byte);
            var typeByteNullable = typeof(byte?);
            var typeDecimal = typeof(decimal);
            var typeDecimalNullable = typeof(decimal?);
            var typeDouble = typeof(double);
            var typeDoubleNullable = typeof(double?);
            var typeFloat = typeof(float);
            var typeFloatNullable = typeof(float?);
            var typeInt = typeof(int);
            var typeIntNullable = typeof(int?);
            var typeLong = typeof(long);
            var typeLongNullable = typeof(long?);
            var typeShort = typeof(short);
            var typeShortNullable = typeof(short?);
            var typeString = typeof(string);
            var typeEnumerable = typeof(TResult);

            var searchValueItems = searchValue.Split(" ");
            List<string> bodyMemberNames;

            if (searchFilter != null)
            {
                bodyMemberNames = searchFilter.Body.GetMemberNames();
            }
            else
            {
                bodyMemberNames = selector.Body.GetMemberNames();
            }

            Expression expressionBody = null;
            var modelParameter = Expression.Parameter(typeEnumerable, "x");

            queryableSelect.LookupProperties((propertyInfo, fullPropertyName) =>
            {
                var propertyType = propertyInfo.PropertyType;
                var fullPropertyNameSplit = fullPropertyName.Split(".");

                var isValidType = true;
                Expression expressionSubBody = null;
                MemberExpression memberExpression = modelParameter.GetMemberExpression(fullPropertyNameSplit);

                if (propertyType == typeByteNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeByteNullable));
                }
                else if (propertyType == typeDecimalNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeDecimalNullable));
                }
                else if (propertyType == typeDoubleNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeDoubleNullable));
                }
                else if (propertyType == typeFloatNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeFloatNullable));
                }
                else if (propertyType == typeIntNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeIntNullable));
                }
                else if (propertyType == typeLongNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeLongNullable));
                }
                else if (propertyType == typeShortNullable)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeShortNullable));
                }
                else if (propertyType == typeString)
                {
                    expressionSubBody = Expression.NotEqual(memberExpression, Expression.Constant(null, typeString));
                }

                for (var j = 0; j < searchValueItems.Length; j++)
                {
                    var searchValueItem = searchValueItems[j];
                    Expression searchValueExpression;

                    if (
                        (propertyType == typeByte || propertyType == typeByteNullable) &&
                        byte.TryParse(searchValueItem, out byte searchValueItemByte)
                    )
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemByte, typeByte));
                    }
                    else if (
                        (propertyType == typeDecimal || propertyType == typeDecimalNullable) &&
                        decimal.TryParse(searchValueItem, out decimal searchValueItemDecimal)
                    )
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemDecimal, typeDecimal));
                    }
                    else if (
                        (propertyType == typeDouble || propertyType == typeDoubleNullable) &&
                        double.TryParse(searchValueItem, out double searchValueItemDouble)
                    )
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemDouble, typeDouble));
                    }
                    else if (
                        (propertyType == typeFloat || propertyType == typeFloatNullable) &&
                        float.TryParse(searchValueItem, out float searchValueItemFloat))
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemFloat, typeFloat));
                    }
                    else if (
                        (propertyType == typeInt || propertyType == typeIntNullable) &&
                        int.TryParse(searchValueItem, out int searchValueItemInteger))
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemInteger, typeInt));
                    }
                    else if (
                        (propertyType == typeLong || propertyType == typeLongNullable) &&
                        long.TryParse(searchValueItem, out long searchValueItemLong))
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemLong, typeLong));
                    }
                    else if (
                        (propertyType == typeShort || propertyType == typeShortNullable) &&
                        short.TryParse(searchValueItem, out short searchValueItemShort))
                    {
                        searchValueExpression = Expression.Equal(memberExpression, Expression.Constant(searchValueItemShort, typeShort));
                    }
                    else if (propertyType == typeString)
                    {
                        var modelPropertyUpper = Expression.Call(memberExpression, "ToUpper", null);
                        searchValueExpression = Expression.Call(
                            modelPropertyUpper,
                            "Contains",
                            null,
                            Expression.Constant(searchValueItem, typeString)
                        );
                    }
                    else
                    {
                        isValidType = false;

                        break;
                    }

                    if (expressionSubBody != null)
                    {
                        expressionSubBody = Expression.AndAlso(expressionSubBody, searchValueExpression);
                    }
                    else
                    {
                        expressionSubBody = searchValueExpression;
                    }
                }

                if (!isValidType)
                {
                    return;
                }

                if (expressionBody != null)
                {
                    expressionBody = Expression.OrElse(expressionBody, expressionSubBody);
                }
                else
                {
                    expressionBody = expressionSubBody;
                }
            }, 3, null, bodyMemberNames);

            var parameters = new ParameterExpression[]
            {
                modelParameter
            };

            var expressionLambda = Expression.Lambda<Func<TResult, bool>>(expressionBody, parameters);
            queryableSelect = queryableSelect.Where(expressionLambda);

            return queryableSelect;
        }

        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> queryable, Expression<Func<TSource, TResult>> selector, string searchValue, Expression<Func<TSource, TResult>> searchFilter = null)
        {
            return queryable.Select<TSource, TResult, TResult>(selector, searchValue, searchFilter);
        }

        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> queryable)
        {
            if (!(queryable is IAsyncEnumerable<TSource>))
            {
                return Task.FromResult(queryable.ToList());
            }

            return queryable.ToListAsync();
        }

        // IMPORTANT: If neccessary, place the .Select before .Where, especially when working with related tables
        // .Include has no effect on .Where, the only way to work with related tables is by selecting its properties before filtering

        public static IQueryable<T> WhereSearchValue<T>(this IQueryable<T> queryable, Func<T, object[]> searchValuePredicate = null, string searchValue = null)
        {
            if (searchValuePredicate == null || searchValue == null || searchValue == "")
            {
                return queryable;
            }

            var searchValues = searchValue.Split(" ").ToList();
            var indexEmpty = searchValues.IndexOf("");

            while (indexEmpty != -1)
            {
                searchValues.RemoveAt(indexEmpty);

                indexEmpty = searchValues.IndexOf("");
            }

            return queryable.Where(x => searchValues.All(y => y != null ?
                (searchValuePredicate(x) != null ?
                    searchValuePredicate(x).Any(z => z != null ? (z + "").ToUpper().Contains(y.ToUpper()) :false) :
                false) :
            false));
        }

        public static IQueryable<T> WhereSearchValue<T>(this IQueryable<T> queryable, Func<T, string[]> searchValuePredicate = null, string searchValue = null)
        {
            if (searchValuePredicate == null || searchValue == null || searchValue == "")
            {
                return queryable;
            }

            var searchValues = searchValue.Split(" ").ToList();
            var indexEmpty = searchValues.IndexOf("");

            while (indexEmpty != -1)
            {
                searchValues.RemoveAt(indexEmpty);

                indexEmpty = searchValues.IndexOf("");
            }

            return queryable.Where(x => searchValuePredicate(x).Any(y => y.ToUpper().Contains(y.ToUpper())));

            //return queryable.Where(x => searchValues.All(y => y != null ?
            //    (searchValuePredicate(x) != null ?
            //        searchValuePredicate(x).Any(z => z != null ? z.ToUpper().Contains(y.ToUpper()) : false) :
            //    false) :
            //false));
        }

        /// <summary>
        /// Realiza una busqueda Full-Text en los campos indicados. (Debe habilitar Full-Text en las columnas)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searchValuePredicate"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereSearchFullText<T>(this IQueryable<T> queryable,
            string searchValue = null,
            string[] searchValuePredicate = null
            //Func<T, string[]> searchValuePredicate = null
            )
        {
            if (searchValuePredicate == null || string.IsNullOrEmpty(searchValue)) return queryable;
            searchValue = $"\"{searchValue}*\"";
            //var r = queryable.First(x => x);

            //queryable.Where(x => searchValuePredicate)

            foreach (var item in searchValuePredicate)
            {
                queryable = queryable.Where(x => EF.Functions.Contains(item, searchValue));
            }

            //searchValuePredicate(queryable.GetType())
            //queryable.ForEachAsync()
            //return queryable.Where(x => searchValuePredicate(x).Any(y => EF.Functions.Contains(y, searchValue)));
            return queryable;
        }
    }
}
