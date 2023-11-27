using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VQuestion : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid VExamId { get; set; }
        public VExam VExam { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9][0-9][0-9]?)?")]
        public decimal A { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9][0-9][0-9]?)?")]
        public decimal H { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9][0-9][0-9]?)?")]
        public decimal W { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]")]
        public int? Count { get; set; } //para calculada y calculada opcion multiple
        public string Description { get; set; }
        public string Format { get; set; }
        public string ImageRoute { get; set; } //para arrastrar y soltar sobre imagen
        public string FeedbackHigh { get; set; }
        public string FeedbackLow { get; set; }
        public string FeedbackMedium { get; set; }
        public string Type { get; set; }

        [RegularExpression(@"[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?")]
        public decimal Value { get; set; }
        public byte RubricType { get; set; } = 0;
        public Guid? ExamSegmentId { get; set; }
        public ExamSegment ExamSegment { get; set; }
        public TimeSpan? Duration { get; set; }
        public virtual ICollection<Alternative> Alternatives { get; set; } = new HashSet<Alternative>();
        public virtual ICollection<Calculated> VariablesCalculated { get; set; }
        public virtual ICollection<CalculatedAlternative> AlternativesCalculated { get; set; } = new HashSet<CalculatedAlternative>();
        public virtual ICollection<GroupChoice> GroupChoices { get; set; } = new HashSet<GroupChoice>();
        public virtual ICollection<Number> Numbers { get; set; } = new HashSet<Number>();
        public virtual ICollection<SubImage> SubImagens { get; set; } = new HashSet<SubImage>();
        public virtual ICollection<SubInput> SubInputs { get; set; } = new HashSet<SubInput>();
        public virtual ICollection<SubQuestion> SubQuestions { get; set; } = new HashSet<SubQuestion>();
        public virtual ICollection<Variable> Variables { get; set; } = new HashSet<Variable>();
        public virtual ICollection<QuestionRubric> QuestionRubrics { get; set; } = new HashSet<QuestionRubric>();
    }
}