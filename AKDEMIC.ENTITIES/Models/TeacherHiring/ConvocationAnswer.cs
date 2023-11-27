using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationAnswer
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid ConvocationQuestionId { get; set; }
        public ConvocationQuestion ConvocationQuestion { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUser { get; set; }
    }
}
