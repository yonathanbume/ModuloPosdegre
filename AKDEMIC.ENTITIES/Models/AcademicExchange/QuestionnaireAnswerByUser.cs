using System;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class QuestionnaireAnswerByUser 
    {
        public Guid Id { get; set; }

        public Guid QuestionnaireQuestionId { get; set; }
        public QuestionnaireQuestion QuestionnaireQuestion { get; set; }

        public string AnswerDescription { get; set; }

        public Guid? QuestionnaireAnswerId { get; set; }
        public QuestionnaireAnswer QuestionnaireAnswer { get; set; }

        public Guid PostulationId { get; set; }
        public Postulation Postulation { get; set; }
    }
}
