using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class AcademicDepartmentMap
    {
        public AcademicDepartmentMap(EntityTypeBuilder<AcademicDepartment> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.Name).HasMaxLength(100);

            entityBuilder.Property(x => x.Status).IsRequired();

            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
