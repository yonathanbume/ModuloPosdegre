using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleSectionAnnexMap
    {
        public ScaleSectionAnnexMap(EntityTypeBuilder<ScaleSectionAnnex> entityBuilder)
        {
            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
