using System;
using System.Reflection;
using AKDEMIC.ENTITIES.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Helpers
{
    public static class ModelBuilderHelpers
    {
        public static MethodInfo GetMethodInfo(string methodInfoName)
        {
            MethodInfo[] methodInfos = typeof(ModelBuilderHelpers).GetMethods(BindingFlags.Public | BindingFlags.Static);

            for (int i = 0; i < methodInfos.Length; i++)
            {
                MethodInfo methodInfo = methodInfos[i];

                if (methodInfo.IsGenericMethod && methodInfo.Name == methodInfoName)
                {
                    return methodInfo;
                }
            }

            return null;
        }

        public static void CodeNumberProperty<T>(ModelBuilder modelBuilder) where T : class, ICodeNumber
        {
            modelBuilder.Entity<T>()
                .Property<int>("GeneratedId")
                .ValueGeneratedOnAdd();
        }

        public static void CodeNumberHasAlternateKey<T>(ModelBuilder modelBuilder) where T : class, ICodeNumber
        {
            modelBuilder.Entity<T>()
                .HasAlternateKey("GeneratedId");
        }

        public static void KeyNumberProperty<T>(ModelBuilder modelBuilder) where T : class, IKeyNumber
        {
            modelBuilder.Entity<T>()
                .Property<string>("RelationId")
                .HasMaxLength(50);
        }

        public static void SoftDeleteHasQueryFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
        {
            modelBuilder.Entity<T>()
                .HasQueryFilter(filter => EF.Property<DateTime?>(filter, "DeletedAt") == null);

            modelBuilder.Entity<T>()
                .HasQueryFilter(filter => EF.Property<string>(filter, "DeletedBy") == null);
        }

        public static void SoftDeleteProperty<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
        {
            modelBuilder.Entity<T>()
                .Property<DateTime?>("DeletedAt");

            modelBuilder.Entity<T>()
                .Property<string>("DeletedBy");
        }

        public static void TimestampProperty<T>(ModelBuilder modelBuilder) where T : class, ITimestamp
        {
            modelBuilder.Entity<T>()
                .Property<DateTime?>("CreatedAt");

            modelBuilder.Entity<T>()
                .Property<DateTime?>("UpdatedAt");

            modelBuilder.Entity<T>()
                .Property<string>("CreatedBy");

            modelBuilder.Entity<T>()
                .Property<string>("UpdatedBy");
        }

        public static void TrackNumberProperty<T>(ModelBuilder modelBuilder) where T : class, ITrackNumber
        {
            modelBuilder.Entity<T>()
                .Property<string>("SearchId")
                .HasMaxLength(50);
        }
    }
}