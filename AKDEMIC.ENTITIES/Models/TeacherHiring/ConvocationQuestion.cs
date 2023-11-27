using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationQuestion : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }
        public Guid ConvocationSectionId { get; set; }
        public ConvocationSection ConvocationSection { get; set; }
        public ICollection<ConvocationAnswer> ConvocationAnswers { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
    }
}
