using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class Questionnaire
    {
        public Guid Id { get; set; }
        public Guid ScholarshipId { get; set; }
        public Scholarship Scholarship { get; set; }
        public ICollection<QuestionnaireSection> QuestionnaireSections { get; set; }
        public ICollection<Postulation> Postulations { get; set; }
    }
}
