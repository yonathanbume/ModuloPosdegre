using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleResolutionTypeMap
    {
        public ScaleResolutionTypeMap(EntityTypeBuilder<ScaleResolutionType> entityBuilder)
        {
            entityBuilder.HasIndex(p => p.Name)
                .IsUnique();

            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
