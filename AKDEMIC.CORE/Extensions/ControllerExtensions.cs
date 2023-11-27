using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class ControllerExtensions
    {
        //public static Task<byte[]> GetFunction<T>(this T thisC, string functionName, params object[] parameters) where T : class
        //{
        //    Type thisType = thisC.GetType();
        //    MethodInfo theMethod = thisType.GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);

        //    var bytess = (Task<byte[]>)theMethod.Invoke(thisC, parameters);
        //    return bytess;
        //}

        public static Task<U> GetFunction<T, U>(this T thisC, string functionName, params object[] parameters) where T : class
        {
            Type thisType = thisC.GetType();
            MethodInfo theMethod = thisType.GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);

            var functionReturned = (Task<U>)theMethod.Invoke(thisC, parameters);
            return functionReturned;
        }
    }
}
