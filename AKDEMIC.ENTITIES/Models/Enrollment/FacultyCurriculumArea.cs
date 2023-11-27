using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class FacultyCurriculumArea
    {
        public Guid Id { get; set; }
        public Guid CurriculumAreaId { get; set; }
        public Guid FacultyId { get; set; }

        public CurriculumArea CurriculumArea { get; set; }
        public Faculty Faculty { get; set; }
    }
}
