using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Group
    {
        public Guid Id { get; set; }
        //public Guid AcademicYearId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
        public byte Vacancies { get; set; } = 0;

        //public AcademicYear AcademicYear { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
    }
}