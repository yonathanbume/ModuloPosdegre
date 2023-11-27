using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SurveyItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsLikert { get; set; } //si es o no escala likert
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }
        public ICollection<Question> Questions { get; set; }

    }
}
