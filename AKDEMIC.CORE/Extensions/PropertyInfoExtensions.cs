using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace AKDEMIC.CORE.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasCustomAttribute(this PropertyInfo propertyInfo, Type attributeType, bool inherit = true)
        {
            return propertyInfo.GetCustomAttribute(attributeType, inherit) != null;
        }

        public static bool HasCustomAttributes(this PropertyInfo propertyInfo, List<Type> attributeTypes, bool inherit = true)
        {
            for (var i = 0; i < attributeTypes.Count; i++)
            {
                if (propertyInfo.HasCustomAttribute(attributeTypes[i], inherit))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Lookup(this PropertyInfo propertyInfo, Action<PropertyInfo, string> action, int depth = 3, string category = null, List<string> memberNames = null, List<Type> baseTypes = null, List<Type> blockedAttributeTypes = null)
        {

            if (blockedAttributeTypes == null)
            {
                blockedAttributeTypes = new List<Type> {
                    typeof(NotMappedAttribute)
                };
            }

            if (propertyInfo.HasCustomAttributes(blockedAttributeTypes))
            {
                return;
            }

            if (baseTypes == null)
            {
                baseTypes = new List<Type> {
                    typeof(byte),
                    typeof(byte?),
                    typeof(decimal),
                    typeof(decimal?),
                    typeof(double),
                    typeof(double?),
                    typeof(float),
                    typeof(float?),
                    typeof(int),
                    typeof(int?),
                    typeof(long),
                    typeof(long?),
                    typeof(short),
                    typeof(short?),
                    typeof(string)
                };
            }

            var categorySeparator = false;
            var processedDepth = depth;

            if (category != null && category != "")
            {
                categorySeparator = true;
            }

            if (depth > 0)
            {
                processedDepth--;
            }

            category = $"{category}{(categorySeparator ? "." : "")}{propertyInfo.Name}";

            void Search(PropertyInfo internalPropertyInfo)
            {
                var typeProperties = internalPropertyInfo.PropertyType.GetProperties();
                var typePropertiesLength = typeProperties.Length;

                if (typePropertiesLength > 0)
                {
                    for (var j = 0; j < typePropertiesLength; j++)
                    {
                        var typeProperty = typeProperties[j];
                        var propertyPropertyType = typeProperty.PropertyType;

                        if (depth != 0 && typeProperty.Name != internalPropertyInfo.Name && typeProperty.PropertyType != internalPropertyInfo.PropertyType)
                        {
                            if (baseTypes.Contains(propertyPropertyType))
                            {
                                action(propertyInfo, category);
                            }
                            else
                            {
                                typeProperty.Lookup(action, processedDepth, category, memberNames, baseTypes);
                            }
                        }
                    }
                }
                else
                {
                    action(propertyInfo, category);
                }
            }

            var memberNamesCount = memberNames.Count;

            if (memberNames != null && memberNamesCount > 0)
            {
                for (var i = 0; i < memberNamesCount; i++)
                {
                    var memberName = memberNames[i];

                    if (memberName.StartsWith(category))
                    {
                        if (category.Split(".").Length >= memberName.Split(".").Length)
                        {
                            action(propertyInfo, category);
                        }
                        else
                        {
                            Search(propertyInfo);
                        }
                    }
                }
            }
            else
            {
                Search(propertyInfo);
            }
        }
    }
}
