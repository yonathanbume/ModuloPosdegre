using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static AKDEMIC.CORE.Extensions.MutableRelationalNameExtensions;

namespace AKDEMIC.CORE.Extensions
{
    public static class IMutableKeyExtensions
    {
        public static IMutableKey NormalizeRelationalName(this IMutableKey mutableKey, int length = -1)
        {
            var mutableKeyRelational = mutableKey;
            MutableRelationalName mutableKeyRelationalName = mutableKeyRelational.GetName();
            var normalizedRelationalName = mutableKeyRelationalName.NormalizeRelationalName();

            if (length >= 0 && normalizedRelationalName.Length >= length)
            {
                normalizedRelationalName = normalizedRelationalName.Substring(0, length);
            }

            mutableKeyRelational.SetName(normalizedRelationalName);

            return mutableKey;
        }
    }
}
