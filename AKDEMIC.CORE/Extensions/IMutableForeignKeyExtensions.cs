using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static AKDEMIC.CORE.Extensions.MutableRelationalNameExtensions;

namespace AKDEMIC.CORE.Extensions
{
    public static class IMutableForeignKeyExtensions
    {
        public static IMutableForeignKey RestrictDeleteBehavior(this IMutableForeignKey mutableForeignKey)
        {
            if (!mutableForeignKey.IsOwnership && mutableForeignKey.DeleteBehavior == DeleteBehavior.Cascade)
            {
                mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            return mutableForeignKey;
        }

        public static IMutableForeignKey NormalizeRelationalName(this IMutableForeignKey mutableForeignKey, int length = -1)
        {
            var mutableForeignKeyRelational = mutableForeignKey;
            MutableRelationalName mutableKeyRelationalName = mutableForeignKeyRelational.GetConstraintName();
            var normalizedRelationalName = mutableKeyRelationalName.NormalizeRelationalName();

            if (length >= 0 && normalizedRelationalName.Length >= length)
            {
                normalizedRelationalName = normalizedRelationalName.Substring(0, length);
            }

            mutableForeignKeyRelational.SetConstraintName(normalizedRelationalName);

            return mutableForeignKey;
        }
    }
}
