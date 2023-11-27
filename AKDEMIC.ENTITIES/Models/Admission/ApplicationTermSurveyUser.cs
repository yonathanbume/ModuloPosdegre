using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermSurveyUser : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid PostulantId { get; set; }
        public Postulant Postulant { get; set; }
        public Guid ApplicationTermSurveyId { get; set; }
        public ApplicationTermSurvey ApplicationTermSurvey { get; set; }
        public ICollection<ApplicationTermSurveyAnswerByUser> ApplicationTermSurveyAnswerByUsers { get; set; }
    }
}
