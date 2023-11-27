using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationSection : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public ICollection<ConvocationQuestion> ConvocationQuestions { get; set; }
    }
}
