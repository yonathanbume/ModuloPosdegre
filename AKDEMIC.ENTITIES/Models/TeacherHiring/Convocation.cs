using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class Convocation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal MinScore { get; set; }
        public string Requirements { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ConvocationAcademicDeparment> ConvocationAcademicDeparments { get; set; }
        public ICollection<ConvocationComitee> ConvocationComitees { get; set; }
        public ICollection<ConvocationCalendar> ConvocationCalendars { get; set; }
        public ICollection<ConvocationSection> ConvocationSections { get; set; }
        public ICollection<ConvocationDocument> ConvocationDocuments { get; set; }
    }
}
