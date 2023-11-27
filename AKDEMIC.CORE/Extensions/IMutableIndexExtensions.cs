using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static AKDEMIC.CORE.Extensions.MutableRelationalNameExtensions;

namespace AKDEMIC.CORE.Extensions
{
    public static class IMutableIndexExtensions
    {
        public static IMutableIndex NormalizeRelationalName(this IMutableIndex mutableIndex, int length = -1)
        {
            var mutableIndexRelational = mutableIndex;
            MutableRelationalName mutableKeyRelationalName = mutableIndexRelational.GetDatabaseName();
            var normalizedRelationalName = mutableKeyRelationalName.NormalizeRelationalName();

            if (length >= 0 && normalizedRelationalName.Length >= length)
            {
                normalizedRelationalName = normalizedRelationalName.Substring(0, length);
            }

            mutableIndexRelational.SetDatabaseName(normalizedRelationalName);

            return mutableIndex;
        }
    }
}
