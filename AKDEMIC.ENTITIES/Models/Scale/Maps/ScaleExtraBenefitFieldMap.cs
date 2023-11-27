using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleExtraBenefitFieldMap
    {
        public ScaleExtraBenefitFieldMap(EntityTypeBuilder<ScaleExtraBenefitField> entityBuilder)
        {
            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
