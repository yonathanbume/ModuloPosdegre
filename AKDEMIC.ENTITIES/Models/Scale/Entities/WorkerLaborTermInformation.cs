using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerLaborTermInformation
    {
        public Guid Id { get; set; }
        public Guid TermId { get; set; }
        public string UserId { get; set; }
        public Guid? WorkerCapPositionId { get; set; }
        public Guid? WorkerLaborCategoryId { get; set; }
        public Guid? WorkerLaborConditionId { get; set; }
        public Guid? WorkerLaborRegimeId { get; set; }
        public Guid? WorkerManagementPositionId { get; set; }
        public Guid? WorkerPositionClassificationId { get; set; }

        public ApplicationUser User { get; set; }
        public Term Term { get; set; }
        public WorkerCapPosition WorkerCapPosition { get; set; }
        public WorkerLaborCategory WorkerLaborCategory { get; set; }
        public WorkerLaborCondition WorkerLaborCondition { get; set; }
        public WorkerLaborRegime WorkerLaborRegime { get; set; }
        public WorkerManagementPosition WorkerManagementPosition { get; set; }
        public WorkerPositionClassification WorkerPositionClassification { get; set; }
    }
}
