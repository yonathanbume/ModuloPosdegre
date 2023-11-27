using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermSurveyAnswer
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermSurveyQuestionId { get; set; }
        public ApplicationTermSurveyQuestion ApplicationTermSurveyQuestion { get; set; }
        public string Description { get; set; }

        public ICollection<ApplicationTermSurveyAnswerByUser> ApplicationTermSurveyAnswerByUsers { get; set; }
    }
}
