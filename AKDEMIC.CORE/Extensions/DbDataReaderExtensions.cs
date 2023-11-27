using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static async Task<DateTime?> TryGetDateTime(this DbDataReader dbDataReader, int ordinal, CancellationToken cancellationToken = default(CancellationToken))
        {
            DateTime? dateTime = null;

            if (ordinal >= 0)
            {
                var isDBNull = await dbDataReader.IsDBNullAsync(ordinal, cancellationToken);

                if (!isDBNull)
                {
                    dateTime = dbDataReader.GetDateTime(ordinal);
                }
            }

            return dateTime;
        }

        public static async Task<DateTime?> TryGetDateTime(this DbDataReader dbDataReader, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordinal = await dbDataReader.TryGetOrdinal(name);
            var dateTime = await TryGetDateTime(dbDataReader, ordinal, cancellationToken);

            return dateTime;
        }

        public static async Task<int?> TryGetInt32(this DbDataReader dbDataReader, int ordinal, CancellationToken cancellationToken = default(CancellationToken))
        {
            int? int32 = null;

            if (ordinal >= 0)
            {
                var isDBNull = await dbDataReader.IsDBNullAsync(ordinal, cancellationToken);

                if (!isDBNull)
                {
                    int32 = dbDataReader.GetInt32(ordinal);
                }
            }

            return int32;
        }

        public static async Task<int?> TryGetInt32(this DbDataReader dbDataReader, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordinal = await dbDataReader.TryGetOrdinal(name);
            var int32 = await TryGetInt32(dbDataReader, ordinal, cancellationToken);

            return int32;
        }

        public static async Task<int> TryGetOrdinal(this DbDataReader dbDataReader, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordinal = -1;
            var ordinal2 = dbDataReader.GetOrdinal(name);

            if (!await dbDataReader.IsDBNullAsync(ordinal2))
            {
                ordinal = ordinal2;
            }

            return ordinal;
        }

        public static async Task<string> TryGetString(this DbDataReader dbDataReader, int ordinal, CancellationToken cancellationToken = default(CancellationToken))
        {
            string s = null;

            if (ordinal >= 0)
            {
                var isDBNull = await dbDataReader.IsDBNullAsync(ordinal, cancellationToken);

                if (!isDBNull)
                {
                    s = dbDataReader.GetString(ordinal);
                }
            }

            return s;
        }

        public static async Task<string> TryGetString(this DbDataReader dbDataReader, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordinal = await dbDataReader.TryGetOrdinal(name);
            var s = await TryGetString(dbDataReader, ordinal, cancellationToken);

            return s;
        }

        public static async Task<object> TryGetValue(this DbDataReader dbDataReader, int ordinal, CancellationToken cancellationToken = default(CancellationToken))
        {
            object obj = null;

            if (ordinal >= 0)
            {
                var isDBNull = await dbDataReader.IsDBNullAsync(ordinal, cancellationToken);

                if (!isDBNull)
                {
                    obj = dbDataReader.GetValue(ordinal);
                }
            }

            return obj;
        }

        public static async Task<object> TryGetValue(this DbDataReader dbDataReader, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ordinal = await dbDataReader.TryGetOrdinal(name);
            var obj = await TryGetValue(dbDataReader, ordinal, cancellationToken);

            return obj;
        }
    }
}
