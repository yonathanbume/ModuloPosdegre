using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleSectionMap
    {
        public ScaleSectionMap(EntityTypeBuilder<ScaleSection> entityBuilder)
        {
            entityBuilder
                .HasIndex(p => p.SectionNumber)
                .IsUnique();
        }
    }
}
