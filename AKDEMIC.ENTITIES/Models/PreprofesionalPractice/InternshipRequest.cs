using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class InternshipRequest : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public byte Status { get; set; }

        public string WorkCenter { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string Schedule { get; set; }

        public string DutyManager { get; set; }

        public string DutyManagerInformation { get; set; }

        public DateTime? CareerDirectorValidationDate { get; set; }
        public string CareerDirectorId { get; set; }
        public ApplicationUser CareerDirector { get; set; }

        public DateTime? DepartmentDirectorValidationDate { get; set; }
        public string DepartmentDirectorId { get; set; }
        public ApplicationUser DepartmentDirector { get; set; }

        public string AdviserSuggestionId { get; set; }
        public ApplicationUser AdviserSuggestion { get; set; }

        public string AdviserId { get; set; }
        public ApplicationUser Adviser { get; set; }

        public DateTime? DevelopmentPlanDateTime { get; set; }
        public DateTime? PartialReportDateTime { get; set; }
        public DateTime? FinalReportDateTime { get; set; }

        public DateTime? QualificationDateTime { get; set; }
        public string WorkplaceAssessmentFile { get; set; }
        public bool? EnabledToGenerateConstancy { get; set; }
        public int? ApprovedCriteriaWorkplaceAssessment { get; set; }
        public ICollection<InternshipRequestFile> InternshipRequestFiles { get; set; }
    }
}
