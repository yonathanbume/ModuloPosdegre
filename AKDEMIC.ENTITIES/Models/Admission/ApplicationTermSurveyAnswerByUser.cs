using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermSurveyAnswerByUser
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermSurveyUserId { get; set; }
        public ApplicationTermSurveyUser ApplicationTermSurveyUser { get; set; }
        public Guid ApplicationTermSurveyQuestionId { get; set; }
        public ApplicationTermSurveyQuestion ApplicationTermSurveyQuestion { get; set; }
        public string Description { get; set; }
        public Guid? ApplicationTermSurveyAnswerId { get; set; }
        public ApplicationTermSurveyAnswer ApplicationTermSurveyAnswer { get; set; }
    }
}
