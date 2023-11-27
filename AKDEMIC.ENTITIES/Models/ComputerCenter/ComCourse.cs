using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComCourse : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? PreRequisiteCourseId { get; set; }
        public ComCourse PreRequisiteCourse { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? EnrollmentConceptId { get; set; }
        public Guid? MonthlyPaymentExternalStudentConceptId { get; set; }
        public Guid? MonthlyPaymentInternalStudentConceptId { get; set; }
        public Concept EnrollmentConcept { get; set; }
        public Concept MonthlyPaymentExternalStudentConcept { get; set; }
        public Concept MonthlyPaymentInternalStudentConcept { get; set; }
        public ICollection<ComGroup> ComGroups { get; set; } 
        public ICollection<ComEvaluationCriteria> ComEvaluationCriterias { get; set; }
        public ICollection<ComCourse> PreRequisiteCourses { get; set; }

    }
}
