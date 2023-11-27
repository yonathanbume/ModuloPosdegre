using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleExtraPerformanceEvaluationFieldMap
    {
        public ScaleExtraPerformanceEvaluationFieldMap(EntityTypeBuilder<ScaleExtraPerformanceEvaluationField> entityBuilder)
        {
            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
