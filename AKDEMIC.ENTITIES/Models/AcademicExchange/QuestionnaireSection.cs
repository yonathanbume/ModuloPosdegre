using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class QuestionnaireSection
    {
        public Guid Id { get; set; }
        public bool IsStatic { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }

        [Required]
        public string Title { get; set; }
        public ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    }
}
