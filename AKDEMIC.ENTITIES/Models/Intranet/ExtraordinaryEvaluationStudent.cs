using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtraordinaryEvaluationStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ExtraordinaryEvaluationId { get; set; }
        public decimal Grade { get; set; }
        public DateTime? GradePublicationDate { get; set; }
        public byte Status { get; set; } = ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING;
        public string Observations { get; set; }
        public Guid? AcademicHistoryId { get; set; }
        public Guid? PaymentId { get; set; }
        public Student Student { get; set; }
        public ExtraordinaryEvaluation ExtraordinaryEvaluation { get; set; }
        public AcademicHistory AcademicHistory { get; set; }
        public Payment Payment { get; set; }
    }
}
