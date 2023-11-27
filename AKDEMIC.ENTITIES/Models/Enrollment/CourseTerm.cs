using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseTerm : Entity, IKeyNumber , ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string CoordinatorId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TermId { get; set; }

        public byte Modality { get; set; } = ConstantHelpers.Course.Modality.PRESENTIAL;
        public string Temary { get; set; }
        public int WeekHours { get; set; } = 2;

        public ApplicationUser Coordinator { get; set; }
        public Course Course { get; set; }
        public Term Term { get; set; }

        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<SyllabusTeacher> SyllabusTeachers { get; set; }
    }
}