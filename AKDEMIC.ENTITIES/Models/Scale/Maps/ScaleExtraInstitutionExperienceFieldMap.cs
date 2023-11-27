﻿using AKDEMIC.ENTITIES.Models.Scale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AKDEMIC.ENTITIES.Models.Scale.Maps
{
    public class ScaleExtraInstitutionExperienceFieldMap
    {
        public ScaleExtraInstitutionExperienceFieldMap(EntityTypeBuilder<ScaleExtraInstitutionExperienceField> entityBuilder)
        {
            entityBuilder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
