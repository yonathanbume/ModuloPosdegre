using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class AnswerByUser
    {
        public Guid Id { get; set; }

        public Guid SurveyUserId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        public Answer Answer { get; set; }

        public Question Question { get; set; }

        public SurveyUser SurveyUser { get; set; }
    }
}
