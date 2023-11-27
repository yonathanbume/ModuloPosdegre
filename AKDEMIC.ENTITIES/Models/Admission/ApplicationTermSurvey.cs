using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermSurvey
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationTermSurveyQuestion> ApplicationTermSurveyQuestions { get; set; }
        public ICollection<ApplicationTermSurveyUser> ApplicationTermSurveyUsers { get; set; }

    }
}
