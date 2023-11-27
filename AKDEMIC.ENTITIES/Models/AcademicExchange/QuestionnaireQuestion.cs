using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class QuestionnaireQuestion
    {
        public Guid Id { get; set; }
        public bool IsStatic { get; set; }
        public Guid QuestionnaireSectionId { get; set; }
        public QuestionnaireSection QuestionnaireSection { get; set; }

        [Required]
        public byte Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public byte DescriptionType { get; set; }

        public ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
        public ICollection<QuestionnaireAnswerByUser> QuestionnaireAnswerByUsers { get; set; }
    }
}
