using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CurriculumArea
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<FacultyCurriculumArea> FacultyCurriculumAreas { get; set; }
    }
}
