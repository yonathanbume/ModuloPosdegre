using System;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class AcademicSummary : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public Guid? CurriculumId { get; set; }
        public Guid? CampusId { get; set; }

        public decimal ApprovedCredits { get; set; } = 0;
        public string Observations { get; set; }
        public int StudentAcademicYear { get; set; }
        public int StudentStatus { get; set; } = ConstantHelpers.Student.States.REGULAR;
        public bool TermHasFinished { get; set; }
        public decimal TotalCredits { get; set; }

        public int MeritOrder { get; set; } = 0;
        public int MeritType { get; set; } = ConstantHelpers.ACADEMIC_ORDER.NONE;
        public int TotalOrder { get; set; } = 0;
        public int TotalMeritType { get; set; } = ConstantHelpers.ACADEMIC_ORDER.NONE;

        public bool WasWithdrawn { get; set; } = false;
        public decimal WeightedAverageCumulative { get; set; } = -1;
        public decimal WeightedAverageGrade { get; set; } = -1;

        public decimal TermScore { get; set; }
        public decimal CumulativeScore { get; set; }

        public byte StudentCondition { get; set; } = ConstantHelpers.Student.Condition.REGULAR;
        //[NotMapped]
        public decimal ExtraScore { get; set; }
        //[NotMapped]
        public decimal ExtraCredits { get; set; }

        public Curriculum Curriculum { get; set; }
        public Career Career { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }
        public Campus Campus { get; set; }
    }
}
