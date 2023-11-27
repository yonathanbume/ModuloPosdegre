using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularActivityStudent
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        public Guid ExtracurricularActivityId { get; set; }
        [ForeignKey("ExtracurricularActivityId")]
        public ExtracurricularActivity ExtracurricularActivity { get; set; }
        public int? Grade { get; set; }
        public string UrlFile { get; set; }
        public string Resolution { get; set; }
        public DateTime? EvaluationReportDate { get; set; }
        public DateTime RegisterDate { get; set; }
        [NotMapped] public string RegisterDateText => RegisterDate.ToLocalDateFormat();
    }
}
