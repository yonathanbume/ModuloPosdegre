using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SubstituteExam : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public int? ExamScore { get; set; } // Nota calculada en base a la configuración
        public decimal? TeacherExamScore { get; set; } //Nota colocada por el docente
        public int? PrevFinalScore { get; set; }
        public string PaymentReceipt { get; set; }
        public byte Status { get; set; }
        public string Underpin { get; set; }
        public Guid? SectionId { get; set; }
        public Guid CourseTermId { get; set; }
        public Guid StudentId { get; set; }
        public Guid? SubstituteExamDetailId { get; set; }
        public Section Section { get; set; }
        public CourseTerm CourseTerm { get; set; }
        public Student Student { get; set; }
        public SubstituteExamDetail SubstituteExamDetail { get; set; }
    }
}