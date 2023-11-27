using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermSurveyQuestion : Entity , ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public byte Type { get; set; }
        public Guid ApplicationTermSurveyId { get; set; }
        public ApplicationTermSurvey ApplicationTermSurvey { get; set; }
        public ICollection<ApplicationTermSurveyAnswer> ApplicationTermSurveyAnswers { get; set; }
        public ICollection<ApplicationTermSurveyAnswerByUser> ApplicationTermSurveyAnswerByUsers { get; set; }
    }
}
