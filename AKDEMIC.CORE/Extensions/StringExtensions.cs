using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AKDEMIC.CORE.Extensions
{
    public static class StringExtensions
    {
        public static string TrimSpaces(this string s)
        {
            var stringBuilder = new StringBuilder();
            s = s.Trim();
            s = s.Replace("\n", " ");
            s = s.Replace("\t", " ");

            for (var i = 0; i < s.Length; i++)
            {
                var value = s[i];

                if (i > 0)
                {
                    if (s[i - 1] == ' ' && value == ' ')
                    {
                        continue;
                    }
                }

                stringBuilder.Append(value);
            }

            return stringBuilder.ToString();
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool CleanEquals(this string value, string anotherValue)
        {
            return value.ToLower().Trim().Equals(anotherValue.ToLower().Trim());
        }

        public static bool CleanContains(this string value, string partOfString)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (string.IsNullOrEmpty(partOfString))
                return true;

            return value.ToUpper().Trim().Contains(partOfString.ToUpper().Trim());
        }

        public static bool SplitContains(this string array, string[] otherArray)
        {
            var results = new bool[otherArray.Length];
            string ntext = array.RemoveAccents();
            for (int j = 0; j < otherArray.Length; j++)
            {
                var other = otherArray[j].RemoveAccents();
                results[j] = ntext.CleanContains(other);
            }

            if (results.Any(x => !x))
                return false;

            return true;
        }

        public static string RemoveAccents(this string value)
        {
            var a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            var e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            var i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            var o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            var u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            var A = new Regex("[Á|À|Ä|Â]", RegexOptions.Compiled);
            var E = new Regex("[É|È|Ë|Ê]", RegexOptions.Compiled);
            var I = new Regex("[Í|Ì|Ï|Î]", RegexOptions.Compiled);
            var O = new Regex("[Ó|Ò|Ö|Ô]", RegexOptions.Compiled);
            var U = new Regex("[Ú|Ù|Ü|Û]", RegexOptions.Compiled);
            value = a.Replace(value, "a");
            value = e.Replace(value, "e");
            value = i.Replace(value, "i");
            value = o.Replace(value, "o");
            value = u.Replace(value, "u");
            value = A.Replace(value, "A");
            value = E.Replace(value, "E");
            value = I.Replace(value, "I");
            value = O.Replace(value, "O");
            value = U.Replace(value, "U");
            return value;
        }

        public static string Truncate(this string s, int index, int length = 0)
        {
            if (index >= s.Length) return string.Empty;

            if (s.Length - index < length)
                return s.Substring(index);

            return s.Substring(index, length);
        }
    }
}
