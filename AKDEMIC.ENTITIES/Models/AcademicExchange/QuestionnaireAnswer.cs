using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class QuestionnaireAnswer
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public Guid QuestionnaireQuestionId { get; set; }
        public QuestionnaireQuestion QuestionnaireQuestion { get; set; }

        public ICollection<QuestionnaireAnswerByUser> QuestionnaireAnswerByUsers { get; set; }
    }
}
