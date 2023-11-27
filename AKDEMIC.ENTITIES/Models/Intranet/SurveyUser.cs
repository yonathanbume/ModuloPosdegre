using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SurveyUser : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? SectionId { get; set; }
        public Guid SurveyId { get; set; }
        public string UserId { get; set; }

        public DateTime? DateTime { get; set; }
        public bool IsGraduated { get; set; } = false;
        
        /*REVISAR*/
        public Section Section { get; set; }
        public Survey Survey { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<AnswerByUser> AnswerByUsers { get; set; }
        public ICollection<EmployeeSurvey> EmployeeSurveys { get; set; }
    }
}
