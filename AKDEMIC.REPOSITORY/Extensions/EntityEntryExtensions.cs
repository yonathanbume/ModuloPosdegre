using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public string UserName { get; set; }

        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();
    }

    public static class EntityEntryExtensions
    {
        public static bool HasPropertyEntry(this EntityEntry entityEntry, string propertyName)
        {
            var propertyEntry = entityEntry.Property(propertyName);

            return propertyEntry != null;
        }

        public static void SetCurrentValue(this EntityEntry entityEntry, string propertyName, object currentValue, bool onlyUnmodifiedProperty = true, bool onlyPermanentProperty = true)
        {
            var propertyEntry = entityEntry.Property(propertyName);

            if (
                propertyEntry != null &&
                (!onlyUnmodifiedProperty || (onlyUnmodifiedProperty && !propertyEntry.IsModified)) &&
                (!onlyPermanentProperty || (onlyPermanentProperty && !propertyEntry.IsTemporary))
            )
            {
                propertyEntry.CurrentValue = currentValue;
            }
        }

        public static AuditEntry ToAuditEntry(this EntityEntry entityEntry, string userName)
        {
            if (entityEntry.State != EntityState.Detached && entityEntry.State != EntityState.Unchanged)
            {
                var auditEntry = new AuditEntry(entityEntry)
                {
                    TableName = entityEntry.Metadata.GetTableName(),
                    UserName = userName
                };

                foreach (var propertyEntry in entityEntry.Properties)
                {
                    if (propertyEntry.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(propertyEntry);

                        continue;
                    }

                    var propertyMetadata = propertyEntry.Metadata;
                    var metadataName = propertyMetadata.Name;

                    if (propertyMetadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[metadataName] = propertyEntry.CurrentValue;

                        continue;
                    }

                    switch (entityEntry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[metadataName] = propertyEntry.CurrentValue;

                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[metadataName] = propertyEntry.OriginalValue;

                            break;
                        case EntityState.Modified:
                            if (propertyEntry.IsModified)
                            {
                                auditEntry.NewValues[metadataName] = propertyEntry.CurrentValue;
                                auditEntry.OldValues[metadataName] = propertyEntry.OriginalValue;
                            }

                            break;
                        default:
                            break;
                    }
                }

                return auditEntry;
            }

            return null;
        }
    }
}
