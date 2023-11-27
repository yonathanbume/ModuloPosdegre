using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleVacationAuthorizationMap
    {
        public ScaleVacationAuthorizationMap(EntityTypeBuilder<ScaleVacationAuthorization> entityBuilder)
        {
            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
