using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using Section = AKDEMIC.ENTITIES.Models.Enrollment.Section;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class AcademicHistory : Entity, IKeyNumber, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid? SectionId { get; set; } // DEBE SER NULLABLE PARA SOPORTAR TRASLADOS
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }

        public Guid? EvaluationReportId { get; set; }

        public bool Approved { get; set; }
        public int Grade { get; set; } = 0;

        [StringLength(255)]
        public string Observations { get; set; } = "";
        public int Try { get; set; } = 1;

        [Required]
        public byte Type { get; set; } = ConstantHelpers.AcademicHistory.Types.REGULAR; // 1 Regular 2 Aplazado 3 Curso dirigido
        public bool Withdraw { get; set; }
        public bool Validated { get; set; }

        public Guid? CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }

        public Course Course { get; set; }
        public Section Section { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }
        public EvaluationReport EvaluationReport { get; set; }
    }
}
