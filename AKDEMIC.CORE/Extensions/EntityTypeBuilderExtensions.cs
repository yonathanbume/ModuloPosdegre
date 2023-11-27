using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.CORE.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public class DatabaseType
        {
            public const int MySql = 1;
            public const int Sql = 2;
            public const int PosgreSql = 3;
        }

        public static EntityTypeBuilder<TEntity> ToDatabaseTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, int databaseType, string name, string schema = null) where TEntity : class
        {
            switch (databaseType)
            {
                case DatabaseType.MySql:
                    entityTypeBuilder.ToTable($"{schema ?? "dbo"}_{name}");

                    break;
                case DatabaseType.Sql:
                    entityTypeBuilder.ToTable(name, schema);

                    break;
                case DatabaseType.PosgreSql:
                    entityTypeBuilder.ToTable(name, schema);

                    break;
            }

            return entityTypeBuilder;
        }
    }
}
