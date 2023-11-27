using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Invoke(this ModelBuilder modelBuilder, MethodInfo methodInfo, Type type)
        {
            if (methodInfo != null)
            {
                var methodInfoGeneric = methodInfo.MakeGenericMethod(type);

                methodInfoGeneric.Invoke(modelBuilder, new object[] { modelBuilder });
            }
        }
    }
}
