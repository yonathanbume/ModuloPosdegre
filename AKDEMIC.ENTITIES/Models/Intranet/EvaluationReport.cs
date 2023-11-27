using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class EvaluationReport : Entity, ITimestamp, ICodeNumber
    {
        public Guid Id { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? SectionId { get; set; }
        public string TeacherId { get; set; }
        public Guid? TermId { get; set; }
        public string Code { get; set; }
        public DateTime? LastReportGeneratedDate { get; set; }
        public DateTime? LastGradePublishedDate { get; set; }
        public Guid? EntityId { get; set; }
        [Required]
        public int Number { get; set; } = 1;
        public int? PrintQuantity { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public byte Status { get; set; } = ConstantHelpers.Intranet.EvaluationReport.GENERATED;
        public byte Type { get; set; }
        public Course Course { get; set; }
        public Section Section { get; set; }
        public Teacher Teacher { get; set; }
        public Term Term { get; set; }
    }
}