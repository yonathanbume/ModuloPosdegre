using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.CORE.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKeyTo, TValueTo> ToType<TKeyFrom, TValueFrom, TKeyTo, TValueTo>(this Dictionary<TKeyFrom, TValueFrom> keyValuePairs)
        {
            var result = new Dictionary<TKeyTo, TValueTo>();

            foreach (KeyValuePair<TKeyFrom, TValueFrom> keyValuePair in keyValuePairs)
            {
                var key = (TKeyTo)Convert.ChangeType(keyValuePair.Key, typeof(TKeyTo));
                var value = (TValueTo)Convert.ChangeType(keyValuePair.Value, typeof(TValueTo));

                result.Add(key, value);
            }

            return result;
        }

        public static Dictionary<string, string> ToStringString(this Dictionary<int, string> keyValuePairs)
        {
            return keyValuePairs.ToType<int, string, string, string>();
        }
    }
}
