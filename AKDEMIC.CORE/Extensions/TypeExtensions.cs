using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AKDEMIC.CORE.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            if (type == null)
            {
                yield break;
            }

            var typeInterfaces = type.GetInterfaces();

            foreach (var typeInterface in typeInterfaces)
            {
                yield return typeInterface;
            }

            var currentBaseType = type.BaseType;

            while (currentBaseType != null)
            {
                yield return currentBaseType;

                currentBaseType = currentBaseType.BaseType;
            }
        }

        public static bool IsBool(this Type type)
        {
            var tmpType = Nullable.GetUnderlyingType(type) ?? type;

            return tmpType == typeof(bool);
        }

        public static bool IsDateTime(this Type type)
        {
            var tmpType = Nullable.GetUnderlyingType(type) ?? type;

            return tmpType == typeof(DateTime);
        }

        public static bool IsDecimal(this Type type)
        {
            var tmpType = Nullable.GetUnderlyingType(type) ?? type;

            return tmpType == typeof(decimal);
        }

        public static bool IsEnum(this Type type)
        {
            var tmpType = Nullable.GetUnderlyingType(type) ?? type;

            return tmpType.IsEnum;
        }

        public static bool IsInheritedFromBaseType(this Type type, Type baseType)
        {
            if (type == null)
            {
                return false;
            }

            if (baseType == null)
            {
                return type.IsInterface || type == typeof(object);
            }

            if (baseType.IsInterface)
            {
                var typeInterfaces = type.GetInterfaces();

                return typeInterfaces.Contains(baseType);
            }

            var currentType = type;

            while (currentType != null)
            {
                if (currentType.BaseType == baseType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
        
        public static bool IsGuid(this Type type)
        {
            var tmpType = Nullable.GetUnderlyingType(type) ?? type;

            return tmpType == typeof(Guid);
        }
    }
}
